using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestesWPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> _Dados;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CriarJson();
            if (!string.IsNullOrEmpty(txtBuscar.Text))
            {
                if (_Dados.Count < int.Parse(txtBuscar.Text))
                {
                    MessageBox.Show("Não Existe Resposta para essa Chave.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var x = _Dados[txtBuscar.Text];
                    lblResultado.Content = x;
                }
            }
            else
            {
            }
        }

        private void NovoId(object sender, RoutedEventArgs e)
        {
            CriarJson();
            if (!string.IsNullOrEmpty(txtNovo.Text))
            {
                string Texto = txtNovo.Text;
                AddJson(Texto);
                txtNovo.Text = null;
                MessageBox.Show("Novo Texto Criado Com Sucesso!", "Sucesso!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CriarJson()
        {
            var file = @"Dados\TesteJson.json";
            var data = JsonConvert.DeserializeObject<DadosTextos>(File.ReadAllText(file, Encoding.UTF8));

            _Dados = data.Items.SelectMany(i => i.id, (i, chave) => new { chave, valor = i.texto })
                                    .ToDictionary(i => i.chave, i => i.valor);
        }

        private void AddJson(string Texto)
        {
            bool duplicidadeTexto = false;
            DadosTextos novo = new DadosTextos();
            novo.Items = new List<Textos>();

            Textos NovoTexto = new Textos();
            NovoTexto.texto = Texto;
            int numReg = 0;
            int n = 1;
            string[] newDado = new string[0];
            int tmn = 0;
            int idDuplicidade = 0;

            var file = @"Dados\TesteJson.json";
            var data = JsonConvert.DeserializeObject<DadosTextos>(File.ReadAllText(file, Encoding.UTF8));

            foreach (var dado in data.Items)
            {
                novo.Items.Add(dado);
                if (dado.texto == Texto)
                {
                    tmn = dado.id.Length;
                    newDado = new string[tmn + 1];
                    for (int i = 0; i < tmn; i++)
                    {
                        newDado[i] = dado.id[i];
                    }

                    idDuplicidade = numReg;
                    duplicidadeTexto = true;
                }
                if (int.Parse(dado.id[0]) == n || dado.id.Length > 1/*int.Parse(id[0])*/)
                {
                    n = n + 1 + (dado.id.Length - 1);
                }
                numReg++;
            }

            if (duplicidadeTexto)
            {
                newDado[tmn] = n.ToString();
                data.Items[idDuplicidade].id = newDado;
            }
            else
            {
                NovoTexto.id = new string[] { n.ToString() };
                novo.Items.Add(NovoTexto);
            }



            string output = JsonConvert.SerializeObject(novo);

            File.WriteAllText(@"Dados\TesteJson.json", output.ToString());

            using (StreamWriter file2 = File.CreateText(@"Dados\TesteJson.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file2))
            {
                JObject.Parse(output).WriteTo(writer);
            }
        }
    }
}
