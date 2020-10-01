﻿using System;
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
        private Dictionary<char, string> _Dados;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CriarJson();
            if (!string.IsNullOrEmpty(txtBuscar.Text))
            {
                var x = _Dados[char.Parse(txtBuscar.Text)];
                lblResultado.Content = x;
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
            bool duplicidadeId = false;
            DadosTextos novo = new DadosTextos();
            novo.Items = new List<Textos>();

            Textos NovoTexto = new Textos();
            NovoTexto.texto = Texto;
            int id = 1;

            var file = @"Dados\TesteJson.json";
            var data = JsonConvert.DeserializeObject<DadosTextos>(File.ReadAllText(file, Encoding.UTF8));

            foreach (var dado in data.Items)
            {
                novo.Items.Add(dado);
                if (int.Parse(dado.id) == id)
                {
                    id++;
                }
            }

            NovoTexto.id = id.ToString();
            novo.Items.Add(NovoTexto);

            MessageBox.Show("Novo Texto Criado Com Sucesso!");

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
