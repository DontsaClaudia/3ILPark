using _3ILPark.Models;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Management;
using System.Net.Http;
using System.Net.Http.Json;

namespace _3ILPark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Computers computers = new Computers();
        string? osVersion = string.Empty;
        string? manufacturer = string.Empty;
        string? model = string.Empty;
        string? totalPhysicalMemory = string.Empty;
        string? computerName = string.Empty;

        Network networks = new Network();
        string? nameNet = string.Empty;
        string? description = string.Empty;
        string? macAddress = string.Empty;
        int networkSpeed = 0;
        string[]? gateway;





        public MainWindow()
        {
            InitializeComponent();
            GetSystemInfo();
            GetNetworkInfo();
            SendComputers();
            SendNetworkInfo();
            Visibility = Visibility.Visible;

        }

        private void GetSystemInfo()
        {
            try
            {
                // Créer un objet ManagementObjectSearcher pour interroger le système
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

                // Parcourir chaque objet ManagementObject retourné par le rechercheur
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    // Récupérer les propriétés désirées
                    manufacturer = queryObj["Manufacturer"].ToString();
                    model = queryObj["Model"].ToString();
                    totalPhysicalMemory = queryObj["TotalPhysicalMemory"].ToString();
                    computerName = queryObj["Name"].ToString();

                    // Afficher les informations dans la fenêtre WPF
                    systemInfoTextBox.Text += "Manufacturer: " + manufacturer + "\n";
                    systemInfoTextBox.Text += "Model: " + model + "\n";
                    systemInfoTextBox.Text += "Total Physical Memory: " + totalPhysicalMemory + " bytes\n";
                    systemInfoTextBox.Text += "Computer Name: " + computerName + "\n";
                }

                // Obtenir des informations sur le système d'exploitation
                ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject queryObj in osSearcher.Get())
                {
                    osVersion = queryObj["Caption"].ToString();
                    string? currentUser = queryObj["RegisteredUser"].ToString();
                    byte freePhysicalMemory = (byte)Convert.ToUInt64(queryObj["FreePhysicalMemory"]);

                    // Afficher les informations sur le système d'exploitation
                    systemInfoTextBox.Text += "OS Version: " + osVersion + "\n";
                    systemInfoTextBox.Text += "Current User: " + currentUser + "\n";
                    systemInfoTextBox.Text += "Free Physical Memory: " + freePhysicalMemory + " bytes\n";
                }
            }
            catch (ManagementException ex)
            {
                // Gérer les exceptions de requête WMI
                MessageBox.Show("An error occurred while querying WMI: " + ex.Message);
            }
        }

        private void GetNetworkInfo()
        {


            // Créer un objet ManagementObjectSearcher pour interroger les adaptateurs réseau
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");



            // Parcourir chaque objet ManagementObject retourné par le rechercheur
            foreach (ManagementObject queryObj in searcher.Get())
            {

                try
                {


                    // Récupérer les propriétés désirées
                    nameNet = queryObj["Name"].ToString();
                    description = queryObj["Description"].ToString();
                    macAddress = queryObj["MACAddress"] != null ? queryObj["MACAddress"].ToString() : string.Empty;
                    networkSpeed = (int)(queryObj["Speed"] != null ? Convert.ToUInt64(queryObj["Speed"]) : 0);
                    //gateway = (string[])queryObj["DefaultIPGateway"];

                    // Afficher les informations dans la fenêtre WPF
                    networkInfoTextBox.Text += "Name " + nameNet + "\n";
                    networkInfoTextBox.Text += "Description: " + description + "\n";
                    networkInfoTextBox.Text += "MACAddress: " + macAddress + "\n";
                    networkInfoTextBox.Text += "Speed: " + networkSpeed + " bps\n\n";
                    // networkInfoTextBox.Text += $"Default Gateway: {gateway}\n\n";

                }
                catch (ManagementException ex)
                {

                }


            }


        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void SendComputers()
        {
            computers.Id = 0;
            computers.MappingId = 1;
            computers.RoomId = 1;
            computers.Caption = osVersion;
            computers.Manufacturer = manufacturer;
            computers.Model = model;
            computers.Name = computerName;
            computers.TotalPhysicalMemory = (int)Convert.ToInt64(totalPhysicalMemory);
            computers.Caption = osVersion;
            HttpClient client = new HttpClient();

            using HttpResponseMessage response = await client.PostAsJsonAsync(
            "https://localhost:7195/api/Computers",
            computers);
        }

        private async void SendNetworkInfo()
        {

            networks.NetworkId = 3;
            networks.ComputerId = 1;
            networks.Name = nameNet;
            networks.MACAddress = macAddress;
            networks.Description = description;
            networks.Speed = (int)(ulong)Convert.ToInt64(networkSpeed);
            //networks.DefaultIPGateway = gateway;
            HttpClient client = new HttpClient();
            /*using HttpResponseMessage response = await client.PostAsJsonAsync(
            "https://localhost:7195/api/Networks",
            networks);*/
        }
    }
}