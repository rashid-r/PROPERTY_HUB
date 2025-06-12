
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;

namespace BOOLOG.Application.Interfaces.RepositoryInterfaces
{
    public interface IMLModelRepository
    {
        
        void SaveModel(
            ITransformer model,
            DataViewSchema modelInputSchema, 
            string modelPath,
            Dictionary<Guid, uint> userGuidToIdMap,
            Dictionary<uint, Guid> userIdToGuidMap,
            Dictionary<Guid, uint> propertyGuidToIdMap,
            Dictionary<uint, Guid> propertyIdToGuidMap,
            ITransformer dataPreparationTransformer,
            DataViewSchema dataPreparationInputSchema 
        );

        
        ITransformer LoadModel(
            string modelPath,
            out DataViewSchema modelInputSchema,
            out Dictionary<Guid, uint> userGuidToIdMap,
            out Dictionary<uint, Guid> userIdToGuidMap,
            out Dictionary<Guid, uint> propertyGuidToIdMap,
            out Dictionary<uint, Guid> propertyIdToGuidMap,
            out ITransformer dataPreparationTransformer
        );

       
        bool ModelExists(string modelPath);
    }
}
