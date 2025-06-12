
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Dto.ResponseDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using BOOLOG.Application.Interfaces.RepositoryInterfaces;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOGAM.Domain.Model;

namespace BOOLOG.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly MLContext _mlContext;
        private readonly IMLModelRepository _mlModelRepository;
        private readonly string _modelPath;

        private ITransformer _model;
        private ITransformer _dataPreparationTransformer; 
        private object _modelLock = new object();

        private Dictionary<Guid, uint> _userGuidToIdMap;
        private Dictionary<uint, Guid> _userIdToGuidMap;
        private Dictionary<Guid, uint> _propertyGuidToIdMap;
        private Dictionary<uint, Guid> _propertyIdToGuidMap;

        public RecommendationService(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMLModelRepository mlModelRepository)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _mlContext = new MLContext();
            _mlModelRepository = mlModelRepository;

            _modelPath = Path.Combine(AppContext.BaseDirectory, "MLModels", "recommendationModel.zip");

            InitializeModel();
        }

        private void InitializeModel()
        {
            _userGuidToIdMap = new Dictionary<Guid, uint>();
            _userIdToGuidMap = new Dictionary<uint, Guid>();
            _propertyGuidToIdMap = new Dictionary<Guid, uint>();
            _propertyIdToGuidMap = new Dictionary<uint, Guid>();
            _dataPreparationTransformer = null; 

            if (_mlModelRepository.ModelExists(_modelPath)) 
            {
                try
                {
                    _model = _mlModelRepository.LoadModel(
                        _modelPath,
                        out _, 
                        out _userGuidToIdMap,
                        out _userIdToGuidMap,
                        out _propertyGuidToIdMap,
                        out _propertyIdToGuidMap,
                        out _dataPreparationTransformer 
                    );
                    Console.WriteLine("ML Model and mappings loaded successfully on startup.");

                    if (_dataPreparationTransformer == null)
                    {
                        Console.WriteLine("Data preparation transformer could not be loaded. Model might not be fully functional.");
                        _model = null; 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load existing model or mappings on startup: {ex.Message}. Model needs to be trained.");
                    Console.WriteLine(ex.ToString()); 
                    _model = null;
                    _dataPreparationTransformer = null;
                    _userGuidToIdMap.Clear();
                    _userIdToGuidMap.Clear();
                    _propertyGuidToIdMap.Clear();
                    _propertyIdToGuidMap.Clear();
                }
            }
            else
            {
                Console.WriteLine("No pre-existing ML model or mappings found. Model needs to be trained.");
            }
        }

        public bool IsModelTrained()
        {
            return _model != null && _dataPreparationTransformer != null &&
                   _userGuidToIdMap != null && _userGuidToIdMap.Any() &&
                   _propertyGuidToIdMap != null && _propertyGuidToIdMap.Any();
        }

        public async Task<ApiResponse<string>> TrainModelAsync()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var wishListRepo = scope.ServiceProvider.GetRequiredService<IRepository<WishList>>();
                    var feedbackRepo = scope.ServiceProvider.GetRequiredService<IRepository<Feedback>>();
                    var propertyRepo = scope.ServiceProvider.GetRequiredService<IRepository<Property>>();
                    var userRepo = scope.ServiceProvider.GetRequiredService<IRepository<User>>();

                    var wishLists = (await wishListRepo.GetAllAsync()).ToList();
                    var feedbacks = (await feedbackRepo.GetAllAsync()).ToList();
                    var allUsers = (await userRepo.GetAllAsync()).ToList();
                    var allProperties = (await propertyRepo.GetAllAsync()).ToList();

                    CreateIdMapping(allUsers, allProperties);

                    var trainingData = PrepareTrainingData(wishLists, feedbacks);

                    if (!trainingData.Any())
                    {
                        Console.WriteLine("No interaction data available to train the model.");
                        lock (_modelLock)
                        {
                            _model = null;
                            _dataPreparationTransformer = null;
                            _userGuidToIdMap.Clear();
                            _userIdToGuidMap.Clear();
                            _propertyGuidToIdMap.Clear();
                            _propertyIdToGuidMap.Clear();
                        }
                        return new ApiResponse<string>(400, "No interaction data available to train the model.");
                    }

                    IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                    var dataPreparationPipeline = _mlContext.Transforms.Conversion.MapValueToKey(
                                                        inputColumnName: nameof(PropertyInteraction.UserId),
                                                        outputColumnName: "EncodedUserId")
                                                .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                                                        inputColumnName: nameof(PropertyInteraction.PropertyId),
                                                        outputColumnName: "EncodedPropertyId"));

                    ITransformer fittedDataPreparationTransformer = dataPreparationPipeline.Fit(dataView);
                    IDataView transformedData = fittedDataPreparationTransformer.Transform(dataView);

                    DataViewSchema mainModelInputSchema = transformedData.Schema;

                    DataViewSchema dataPreparationInputSchema = dataView.Schema;


                    var trainingPipeline = _mlContext.Recommendation().Trainers.MatrixFactorization(
                        labelColumnName: nameof(PropertyInteraction.Label),
                        matrixColumnIndexColumnName: "EncodedUserId",
                        matrixRowIndexColumnName: "EncodedPropertyId",
                        numberOfIterations: 20,
                        approximationRank: 100
                    );

                    Console.WriteLine("Training the ML.NET recommendation model...");
                    lock (_modelLock)
                    {
                        _model = trainingPipeline.Fit(transformedData);
                        _dataPreparationTransformer = fittedDataPreparationTransformer; 
                    }
                    Console.WriteLine("Model training complete.");

                    _mlModelRepository.SaveModel(
                        _model,
                        mainModelInputSchema,
                        _modelPath,
                        _userGuidToIdMap,
                        _userIdToGuidMap,
                        _propertyGuidToIdMap,
                        _propertyIdToGuidMap,
                        _dataPreparationTransformer, 
                        dataPreparationInputSchema 
                    );
                }
                return new ApiResponse<string>(200, "Recommendation model trained successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error training model: {ex.Message}");
                Console.WriteLine(ex.ToString()); 

                lock (_modelLock)
                {
                    _model = null;
                    _dataPreparationTransformer = null;
                    _userGuidToIdMap.Clear();
                    _userIdToGuidMap.Clear();
                    _propertyGuidToIdMap.Clear();
                    _propertyIdToGuidMap.Clear();
                }
                return new ApiResponse<string>(500, "Failed to train recommendation model.", ex.Message);
            }
        }

        private void CreateIdMapping(List<User> users, List<Property> properties)
        {
            _userGuidToIdMap = new Dictionary<Guid, uint>();
            _userIdToGuidMap = new Dictionary<uint, Guid>();
            uint currentUserId = 1;
            foreach (var user in users.OrderBy(u => u.Id))
            {
                if (!_userGuidToIdMap.ContainsKey(user.Id))
                {
                    _userGuidToIdMap[user.Id] = currentUserId;
                    _userIdToGuidMap[currentUserId] = user.Id;
                    currentUserId++;
                }
            }

            _propertyGuidToIdMap = new Dictionary<Guid, uint>();
            _propertyIdToGuidMap = new Dictionary<uint, Guid>();
            uint currentPropertyId = 1;
            foreach (var property in properties.OrderBy(p => p.Id))
            {
                if (!_propertyGuidToIdMap.ContainsKey(property.Id))
                {
                    _propertyGuidToIdMap[property.Id] = currentPropertyId;
                    _propertyIdToGuidMap[currentPropertyId] = property.Id;
                    currentPropertyId++;
                }
            }
        }

        private List<PropertyInteraction> PrepareTrainingData(List<WishList> wishLists, List<Feedback> feedbacks)
        {
            var interactions = new List<PropertyInteraction>();

            foreach (var wl in wishLists)
            {
                if (_userGuidToIdMap.ContainsKey(wl.UserId) && _propertyGuidToIdMap.ContainsKey(wl.PropertyId))
                {
                    interactions.Add(new PropertyInteraction
                    {
                        UserId = _userGuidToIdMap[wl.UserId],
                        PropertyId = _propertyGuidToIdMap[wl.PropertyId],
                        Label = 1.0f
                    });
                }
            }

            foreach (var fb in feedbacks)
            {
                if (_userGuidToIdMap.ContainsKey(fb.UserId) && _propertyGuidToIdMap.ContainsKey(fb.PropertyId))
                {
                    interactions.Add(new PropertyInteraction
                    {
                        UserId = _userGuidToIdMap[fb.UserId],
                        PropertyId = _propertyGuidToIdMap[fb.PropertyId],
                        Label = fb.Rating
                    });
                }
            }

            return interactions;
        }

        public async Task<ApiResponse<List<PropertyDto>>> GetRecommendedPropertiesForUserAsync(Guid userGuid, int numberOfRecommendations = 5)
        {
            if (!IsModelTrained())
            {
                return new ApiResponse<List<PropertyDto>>(500, "Recommendation model is not trained or loaded, or mappings are missing.");
            }

            if (!_userGuidToIdMap.TryGetValue(userGuid, out uint userId))
            {
                return new ApiResponse<List<PropertyDto>>(404, $"User with ID '{userGuid}' is not known to the recommendation model. Cannot provide personalized recommendations. Please ensure they have interacted with properties or train the model with this user's data.");
            }

            ITransformer predictionPipeline;
            lock (_modelLock)
            {
                predictionPipeline = _dataPreparationTransformer.Append(_model);
            }

            PredictionEngine<PropertyInteraction, PropertyRecommendation> predictionEngine =
                _mlContext.Model.CreatePredictionEngine<PropertyInteraction, PropertyRecommendation>(predictionPipeline);


            using (var scope = _scopeFactory.CreateScope())
            {
                var propertyRepo = scope.ServiceProvider.GetRequiredService<IRepository<Property>>();
                var wishListRepo = scope.ServiceProvider.GetRequiredService<IRepository<WishList>>();
                var feedbackRepo = scope.ServiceProvider.GetRequiredService<IRepository<Feedback>>();

                var allProperties = (await propertyRepo.GetAllAsync()).ToList();
                var recommendedProperties = new List<(Property Property, float Score)>();

                var userWishlistedPropertyIds = (await wishListRepo.GetAllAsync())
                                                .Where(wl => wl.UserId == userGuid)
                                                .Select(wl => wl.PropertyId)
                                                .ToHashSet();
                var userFeedbackedPropertyIds = (await feedbackRepo.GetAllAsync())
                                                .Where(fb => fb.UserId == userGuid)
                                                .Select(fb => fb.PropertyId)
                                                .ToHashSet();

                foreach (var property in allProperties)
                {
                    if (userWishlistedPropertyIds.Contains(property.Id) || userFeedbackedPropertyIds.Contains(property.Id))
                    {
                        continue;
                    }

                    if (_propertyGuidToIdMap.TryGetValue(property.Id, out uint propertyId))
                    {
                        var prediction = predictionEngine.Predict(new PropertyInteraction
                        {
                            UserId = userId,
                            PropertyId = propertyId,
                            Label = 0 
                        });

                        recommendedProperties.Add((property, prediction.Score));
                    }
                }

                var topRecommendations = recommendedProperties.OrderByDescending(rp => rp.Score)
                                                            .Take(numberOfRecommendations)
                                                            .Select(rp => rp.Property)
                                                            .ToList();

                var recommendedPropertyDtos = _mapper.Map<List<PropertyDto>>(topRecommendations);

                return new ApiResponse<List<PropertyDto>>(200, "Recommendations retrieved successfully.", recommendedPropertyDtos);
            }
        }
    }
}
