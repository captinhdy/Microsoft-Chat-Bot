using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessing;
using NeuralNetwork.Models;


namespace NeuralNetwork
{
    [Serializable]
    public class ClassificationNetwork
    {
        List<Neuron[]> layers;
        Tokenizer tokenizer;
        public List<Neuron[]> Layers
        {
            get { return layers; }
        }

        List<NLPFeatureDataModel> trainingData;
        Random random;
        public List<NLPFeatureDataModel> TrainingData
        {
            get { return trainingData; }
        }

        public ClassificationNetwork()
        {
            trainingData = new List<NLPFeatureDataModel>();
            random = new Random(1);

        }

        public void InitializeNeuralNetwork()
        {
            layers = new List<Neuron[]>();
            int inputLayers = 3;
            int hiddenLayers = 5;
            int hiddenLayer2 = 2;
            int outputLayers = 3;

            Neuron[] hiddenLayer = CreateNetworkLayer(inputLayers, hiddenLayers);
            Neuron[] layer2 = CreateNetworkLayer(hiddenLayers, hiddenLayer2);
            Neuron[] outputLayer = CreateNetworkLayer(hiddenLayers, outputLayers);
            
            layers.Add(hiddenLayer);
            //layers.Add(layer2);
            layers.Add(outputLayer);
        }


        public void GetInitilizationData()
        {
            List<NLPDataModel> rawTrainingData = new List<NLPDataModel>();
            FileProcessing fileProcessing = new FileProcessing();
            tokenizer = new Tokenizer();
            trainingData.Clear();
            rawTrainingData = fileProcessing.ReadCSVLines<NLPDataModel>("NLPBagofWords.csv", true).ToList();
            tokenizer.Keywords = fileProcessing.Headings;

            foreach(NLPDataModel data in rawTrainingData)
            {
                NLPFeatureDataModel feature = new NLPFeatureDataModel();
                

                var properties = typeof(NLPDataModel).GetProperties();
                feature.PhraseFeatures = new double[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    NLPDataModel d = new NLPDataModel();
                    double f = -999;
                    properties[i].GetValue(d);
                    string token = (string)properties[i].GetValue(data);
                    Double.TryParse(token, out f);
                    feature.PhraseFeatures[i] = f;
                }

                int phaseType = -999;
                int.TryParse(data.PhraseType, out phaseType);
                feature.PhaseType = phaseType;
                trainingData.Add(feature);
            }
        }

        public double[] ForwardPropigation(double[] Features)
        {
            double[] inputs = Features;

            for(int i = 0; i < layers.Count; i++)
            {
                double[] newInputs = new double[layers[i].Length];
                for (int j = 0; j < layers[i].Length; j++)
                {
                    var activation = ActivateNeuron(layers[i][j].weight, inputs);
                    layers[i][j].output = Transfer(activation);
                    newInputs[j] = layers[i][j].output;
                }

                inputs = newInputs;

            }

                return inputs;
        }

        public NLPFeatureDataModel NormalizeInput(string InputString)
        {
            NLPFeatureDataModel feature = new NLPFeatureDataModel();
            feature.PhraseFeatures = new double[typeof(NLPDataModel).GetProperties().Length - 1];
            List<string> tokens = tokenizer.TokenizeString(InputString);
            foreach (string token in tokens)
            {
                for(int i = 0; i < tokenizer.Keywords.Count; i++)
                {
                    if(tokenizer.Keywords[i] == token)
                    {
                        feature.PhraseFeatures[i] = 1;
                    }
                }

                
            }

            return feature;
        }

        public void TrainNetwork(List<NLPFeatureDataModel> TrainingSet, double LRate, double Epoch, int Outputs)
        {
            double l_rate = LRate;
            
            for(int i = 0; i < Epoch; i++)
            {
                double sum_error = 0;
                foreach (NLPFeatureDataModel feature in TrainingSet)
                {
                    double[] outputs = ForwardPropigation(feature.PhraseFeatures);
                    double[] expected = new double[outputs.Length];
                    expected[feature.PhaseType] = 1;
                    for (int j = 0; j < outputs.Length; j++)
                    {
                        sum_error += (Math.Pow((expected[j] - outputs[j]), Outputs));
                    }

                    BackwardPropigateError(expected);
                    UpdateWeights(feature, l_rate);
                }
            }
        }

        public async Task UpdateModel(string Phase)
        {
            throw new NotImplementedException();
        }

        private Neuron[] CreateNetworkLayer(int input, int output)
        {
            Neuron[] layer = new Neuron[output];
            for (int i = 0; i < output; i++)
                    {
                        layer[i] = new Neuron();
                        layer[i].weight = new double[input + 1];
                        for (int j = 0; j < input + 1; j++)
                        {
                            layer[i].weight[j] = random.NextDouble();
                        }

                    }

            return layer;
        }

        private double ActivateNeuron(double[] Weights, double[] Inputs)
        {
            double activation = Weights.Last();

            for(int i = 0; i < Weights.Length-1; i++)
            {
                activation += Weights[i] * Inputs[i];
            }

            return activation;
        }

        private double Transfer(double Weight)
        {
            double exp = Math.Exp(-Weight);
            double denominator = (1.0 + exp);
            return 1.0 / denominator;
        }

        private void BackwardPropigateError(double[] Expected)
        {
            for(int i = layers.Count() -1; i >= 0; i--)
            {
                Neuron[] layer = layers[i];
                List<double> errors = new List<double>();
                if( i != (layers.Count -1))
                {
                    for(int j = 0; j < layer.Length; j++)
                    {
                        double error = 0.0;
                        foreach(Neuron neuron in layers[i+1])
                        {
                            error += (neuron.weight[j] * neuron.delta);
                        }
                        errors.Add(error);
                    }
                }
                else
                {
                    for (int j = 0; j < layer.Length; j++)
                    {
                        Neuron neuron = layer[j];
                        errors.Add(Expected[j] - neuron.output);
                    }
                }

                for (int j = 0; j < layer.Length; j++)
                {
                    layers[i][j].delta = errors[j] * TransferDerivative(layers[i][j].output);
                }
            }

            
        }

        void UpdateWeights(NLPFeatureDataModel Feature, double LRate)
        {
            for(int i = 0; i < layers.Count; i++)
            {
                double[] inputs = new double[Feature.PhraseFeatures.Length - 1];
                for (int j = 0; j < Feature.PhraseFeatures.Length - 1; j++)
                {
                    inputs[j] = Feature.PhraseFeatures[j]; //remove last feature
                }

                if (i != 0)
                {
                    inputs = new double[layers[i - 1].Length];
                    for (int j = 0; j < layers[i-1].Length; j++)
                    {
                        inputs[j] = layers[i - 1][j].output;
                    }

                }

                for (int j = 0; j < layers[i].Length; j++)
                {
                    Neuron neuron = layers[i][j];
                    for(int k = 0; k < inputs.Length; k++)
                    {
                        layers[i][j].weight[k] += LRate * layers[i][j].delta * inputs[k];
                    }
                    layers[i][j].weight[layers[i][j].weight.Length -1] += LRate * layers[i][j].delta;
                }
            }
        }

        private double TransferDerivative(double Output)
        {
            return Output * (1.0 - Output);
        }
    }
}
