using Richter.AsyncCancellationTokenAndIProgress;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp
{
    class Program
    {
        #region Attributes 

        private static CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        static async Task Main(string[] args)
        {
            await Task.Factory.StartNew(() => Exemplo());
            await Task.Factory.StartNew(() => Cancelar());
        }

        #region Methods 

        private static async Task Exemplo()
        {
            await Task.Run(() => {
                Console.WriteLine("Pressione qualquer tecla para iniciar o Processo, para cancelar o processo após iniciado tecle ESC.");
                Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            });

            await InitializeProcesso();
        }

        /// <summary>
        /// Processo que executa processo fakes relacionados.
        /// </summary>
        /// <returns>Task</returns>
        private async static Task InitializeProcesso()
        {
            try
            {
                var progress = new Progress<ProgressFeedback>();
                progress.ProgressChanged += Progress_ProgressChanged;

                Task t = new SampleCancellationTokenProgress().Sample(_cts.Token, progress);

                await t
                    .ContinueWith(ascendent =>
                    {
                        Console.WriteLine("Fim do Primeiro Processo.");
                    });

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Cancela o processo.
        /// </summary>
        /// <returns>Task</returns>
        private static async Task Cancelar()
        {
            await Task.Delay(0);

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
            }
            while (keyinfo.Key != ConsoleKey.Escape);
            {
                _cts.Cancel();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Processo cancelado.");
            }
        }
        
        /// <summary>
        /// Atualiza a mensagem que chega pelo IProgress na tela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Struct que contém a mensagem</param>
        private static void Progress_ProgressChanged(object sender, ProgressFeedback e)
        {
            Console.WriteLine(e.Message);
        }

        #endregion
    }
}
