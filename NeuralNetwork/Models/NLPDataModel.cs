using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Models
{
    [Serializable]
    public class NLPDataModel
    {
        public string Create { get; set; }
        public string SOW { get; set; }
        public string Client { get; set; }
        public string PhraseType { get; set; }
    }
}
