using System;
using System.Collections.Generic;
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

namespace TestesWPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSim(object sender, RoutedEventArgs e)
        {
            dynamic x = new { id="001", sim = "Novo plano!", nao = "Ok, certeza que quer sair?", resultado = "sim" };
            ProximoPasso(x, checkSimBase, checkNaoBase, quadroBase);
        }

        private void btnContinuar(object sender, RoutedEventArgs e)
        {
            dynamic proximoPasso = ((Button)sender).Tag;

            ProximoPasso(proximoPasso.x, proximoPasso.checkSimPar, proximoPasso.checkNaoPar, proximoPasso.quadro);
        }
        
        private void ProximoPasso(dynamic resposta, CheckBox checSim, CheckBox checNao, StackPanel QuadroLinha)
        {
            if (checSim.IsChecked == true && checNao.IsChecked == true)
            {
                MessageBox.Show("Escolha apenas uma opção!", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (checSim.IsChecked == true)
            {
                montaStackPanel(resposta.sim, resposta.id);
                QuadroLinha.IsEnabled = false;
            }
            else if (checNao.IsChecked == true)
            {
                montaStackPanel(resposta.nao, resposta.id);
                QuadroLinha.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Escolha uma opção para continuar.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void montaStackPanel(string resposta, string id)
        {
            StackPanel QuadroGeral = new StackPanel();
            StackPanel SubQuadro = new StackPanel { Orientation=Orientation.Horizontal };

            Label texto = new Label { Content = resposta };

            CheckBox checkSim = new CheckBox { Name = "ChecSim" + id, Content = "Sim" };
            CheckBox checkNao = new CheckBox { Name = "ChecNao" + id, Content = "Não" };

            dynamic x = new { id = "002", sim = "Novo plano!2", nao = "Ok, certeza que quer sair?2", resultado = "sim" };
            Button Continuar = new Button { Content = "Continuar", Name="btnContinuar01", Tag=new { checkSimPar = checkSim, checkNaoPar = checkNao, x, quadro = QuadroGeral}};
            Continuar.Click += new RoutedEventHandler(btnContinuar);

            SubQuadro.Children.Add(checkSim);
            SubQuadro.Children.Add(checkNao);
            QuadroGeral.Children.Add(texto);
            QuadroGeral.Children.Add(SubQuadro);
            QuadroGeral.Children.Add(Continuar);
            TelaPrincipal.Children.Add(QuadroGeral);
        }
    }
}
