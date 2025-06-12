
using BOOLOG.Application.Interfaces.RepositoryInterfaces;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class MLModelRepository : IMLModelRepository
{
    private readonly MLContext _mlContext;

    public MLModelRepository(MLContext mlContext)
    {
        _mlContext = mlContext;
    }

    private string GetMappingsPath(string modelPath)
    {
        return Path.ChangeExtension(modelPath, ".json");
    }

    private string GetDataPreparationTransformerPath(string modelPath)
    {
        return Path.Combine(Path.GetDirectoryName(modelPath), "dataPreparationTransformer.zip");
    }

    public void SaveModel(
        ITransformer model,
        DataViewSchema modelInputSchema, 
        string modelPath,
        Dictionary<Guid, uint> userGuidToIdMap,
        Dictionary<uint, Guid> userIdToGuidMap,
        Dictionary<Guid, uint> propertyGuidToIdMap,
        Dictionary<uint, Guid> propertyIdToGuidMap,
        ITransformer dataPreparationTransformer,
        DataViewSchema dataPreparationInputSchema 
    )
    {
        try
        {
            var directory = Path.GetDirectoryName(modelPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _mlContext.Model.Save(model, modelInputSchema, modelPath);
            Console.WriteLine($"ML Model saved to {modelPath}");

            var dataPreparationTransformerPath = GetDataPreparationTransformerPath(modelPath);
            _mlContext.Model.Save(dataPreparationTransformer, dataPreparationInputSchema, dataPreparationTransformerPath);
            Console.WriteLine($"Data Preparation Transformer saved to {dataPreparationTransformerPath}");

            var mappingsData = new MappingsData
            {
                UserGuidToIdMap = userGuidToIdMap,
                UserIdToGuidMap = userIdToGuidMap,
                PropertyGuidToIdMap = propertyGuidToIdMap,
                PropertyIdToGuidMap = propertyIdToGuidMap,
                DataPreparationTransformerFileName = Path.GetFileName(dataPreparationTransformerPath)
            };

            var mappingsPath = GetMappingsPath(modelPath);
            var jsonString = JsonSerializer.Serialize(mappingsData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(mappingsPath, jsonString);
            Console.WriteLine($"Mappings saved to {mappingsPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving ML model or mappings: {ex.Message}");
            Console.WriteLine(ex.ToString());
        }
    }

    public ITransformer LoadModel(
        string modelPath,
        out DataViewSchema modelInputSchema,
        out Dictionary<Guid, uint> userGuidToIdMap,
        out Dictionary<uint, Guid> userIdToGuidMap,
        out Dictionary<Guid, uint> propertyGuidToIdMap,
        out Dictionary<uint, Guid> propertyIdToGuidMap,
        out ITransformer dataPreparationTransformer
    )
    {
        userGuidToIdMap = new Dictionary<Guid, uint>();
        userIdToGuidMap = new Dictionary<uint, Guid>();
        propertyGuidToIdMap = new Dictionary<Guid, uint>();
        propertyIdToGuidMap = new Dictionary<uint, Guid>();
        dataPreparationTransformer = null; 

        if (!ModelExists(modelPath))
        {
            throw new FileNotFoundException($"ML Model or associated files not found at {modelPath}.");
        }

        ITransformer model;
        try
        {
            model = _mlContext.Model.Load(modelPath, out modelInputSchema);
            Console.WriteLine($"ML Model loaded from {modelPath}");

            var mappingsPath = GetMappingsPath(modelPath);
            if (File.Exists(mappingsPath))
            {
                var jsonString = File.ReadAllText(mappingsPath);
                var mappingsData = JsonSerializer.Deserialize<MappingsData>(jsonString);

                userGuidToIdMap = mappingsData.UserGuidToIdMap ?? new Dictionary<Guid, uint>();
                userIdToGuidMap = mappingsData.UserIdToGuidMap ?? new Dictionary<uint, Guid>();
                propertyGuidToIdMap = mappingsData.PropertyGuidToIdMap ?? new Dictionary<Guid, uint>();
                propertyIdToGuidMap = mappingsData.PropertyIdToGuidMap ?? new Dictionary<uint, Guid>();
                Console.WriteLine($"Mappings loaded from {mappingsPath}");

                if (!string.IsNullOrEmpty(mappingsData.DataPreparationTransformerFileName))
                {
                    var dataPreparationTransformerPath = Path.Combine(Path.GetDirectoryName(modelPath), mappingsData.DataPreparationTransformerFileName);
                    if (File.Exists(dataPreparationTransformerPath))
                    {
                        dataPreparationTransformer = _mlContext.Model.Load(dataPreparationTransformerPath, out _);
                        Console.WriteLine($"Data Preparation Transformer loaded from {dataPreparationTransformerPath}");
                    }
                    else
                    {
                        Console.WriteLine($"Data Preparation Transformer file not found at {dataPreparationTransformerPath}. Model might not be fully functional.");
                        dataPreparationTransformer = null; 
                    }
                }
                else
                {
                    Console.WriteLine("Data preparation transformer file name not found in mappings data. Model might not be fully functional.");
                    dataPreparationTransformer = null; 
                }
            }
            else
            {
                Console.WriteLine($"Mappings file not found at {mappingsPath}. Cannot load mappings or data preparation transformer.");
                throw new InvalidOperationException("Mappings file missing, cannot fully load ML model dependencies.");
            }

            if (dataPreparationTransformer == null)
            {
                throw new InvalidOperationException("Data preparation transformer could not be loaded, model cannot be used for predictions.");
            }

            return model;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading ML model or mappings: {ex.Message}");
            throw;
        }
    }

    public bool ModelExists(string modelPath)
    {
        return File.Exists(modelPath) &&
               File.Exists(GetMappingsPath(modelPath)) &&
               File.Exists(GetDataPreparationTransformerPath(modelPath));
    }

    private class MappingsData
    {
        public Dictionary<Guid, uint> UserGuidToIdMap { get; set; }
        public Dictionary<uint, Guid> UserIdToGuidMap { get; set; }
        public Dictionary<Guid, uint> PropertyGuidToIdMap { get; set; }
        public Dictionary<uint, Guid> PropertyIdToGuidMap { get; set; }
        public string DataPreparationTransformerFileName { get; set; }
    }
}
