using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using NeuralNetwork.Models;
using System.Collections.Generic;

namespace NeuralNetwork_UnitTests
{
    [TestClass]
    public class ClassificationNetwork_UnitTests
    {
        ClassificationNetwork network;

        [TestInitialize]
        public void InitializeNetwork()
        {
            
        }

        [TestMethod]
        public void ReadCSV()
        {
            network.GetInitilizationData();
            List<NLPFeatureDataModel> trainingData = network.TrainingData;

            Assert.AreNotEqual(0, trainingData.Count);
        }

        [TestMethod]
        public void InitializeNeuralNetwork()
        {
            network.InitializeNeuralNetwork();
            List<Neuron[]> layers = network.Layers;

            Assert.AreNotEqual(0, layers.Count);
        }


        [TestMethod]
        public void ForwardPropagateNeuralNetwork()
        {
            network.GetInitilizationData();
            network.InitializeNeuralNetwork();
            network.ForwardPropigation(network.TrainingData[0].PhraseFeatures);
        }
        [TestMethod]
        public void TrainNetwork()
        {
            network.GetInitilizationData();
            network.InitializeNeuralNetwork();
            network.TrainNetwork(network.TrainingData, 0.3, (network.TrainingData.Count - 1), 2);

        }

        [TestMethod]
        public void GuessSentence()
        {
            network.GetInitilizationData();
            network.InitializeNeuralNetwork();


                network.TrainNetwork(network.TrainingData, 0.3, (network.TrainingData.Count - 1), 3);


            NLPFeatureDataModel inputData = network.NormalizeInput("Please create an SOW");
            //inputData.PhraseFeatures[0] = 0;
            //inputData.PhraseFeatures[1] = 0;
            //inputData.PhraseFeatures[2] = 0;

            network.ForwardPropigation(inputData.PhraseFeatures);
            Assert.IsTrue(true);
        }
    }
}
