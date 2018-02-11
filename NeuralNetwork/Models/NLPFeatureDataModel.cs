using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Models
{
    [Serializable]
    public struct NLPFeatureDataModel
    {
        public double[] PhraseFeatures { get; set; }
        public int PhaseType { get; set; }
    }
}
