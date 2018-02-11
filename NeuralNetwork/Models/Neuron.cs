using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Models
{
    [Serializable]
    public class Neuron
    {
        public double[] weight { get; set; }
        public double output { get; set; }
        public double delta { get; set; }
    }
}
