using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NeuralNetwork;
using NeuralNetwork.Models;

namespace Chatbot5000
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        ClassificationNetwork network;
        public async Task StartAsync(IDialogContext context)
        {
            network = new ClassificationNetwork();
            network.GetInitilizationData();
            network.InitializeNeuralNetwork();
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            NLPFeatureDataModel inputData = network.NormalizeInput(message.Text);
            double[] guesses = network.ForwardPropigation(inputData.PhraseFeatures);
            string r = "";

            r += "I do not know what you are asking: " + (guesses[0] * 100) + "%";
            r += ", Create an SOW: " + (guesses[1] * 100) + "%";
            r += ", Create a new Client: " + (guesses[2] * 100) + "%";


            if ((guesses[1] * 100) > 90)
            {
                context.Call(new SOWForm(), ResumeAfterOptionDialog);
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}