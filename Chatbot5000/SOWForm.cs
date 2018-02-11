using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Collections;
using System.Resources;
using Microsoft.Bot.Connector;

namespace Chatbot5000
{
    [Serializable]
    public class SOWForm : IDialog<SOWFormModel>
    { 
        public SOWForm()
        {

        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MesssageReceivedAsync);
            //return Task.CompletedTask;
        }

        public virtual async Task MesssageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Call(SOWFormModel.BuildFormDialog(FormOptions.PromptInStart), FormComplete);
        }

        private async Task FormComplete(IDialogContext context, IAwaitable<SOWFormModel> result)
        {
            try
            {
                var form = await result;
                if (form != null)
                {
                    await context.PostAsync("Thanks for completing the form! Just type anything to restart it.");
                }
                else
                {
                    await context.PostAsync("Form returned empty response! Type anything to restart it.");
                }
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("You canceled the form! Type anything to restart it.");
            }

            context.Wait(MesssageReceivedAsync);
        }
    }
}