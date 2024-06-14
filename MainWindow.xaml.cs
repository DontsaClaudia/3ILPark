using _3ILPark.Models;
using System.Net.Http;
using System.Windows;
using System.Management;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using _3ILPark_API.Models;
using Microsoft.VisualBasic.Devices;
using System.Windows.Input;

namespace _3ILPark
{
    public partial class MainWindow : Window
    {
        Computers computers = new Computers();
        string? osVersion = string.Empty;
        string? manufacturer = string.Empty;
        string? model = string.Empty;
        string? totalPhysicalMemory = string.Empty;
        string? computerName = string.Empty;

        Networks networks = new Networks();
        string? nameNet = string.Empty;
        string? description = string.Empty;
        string? macAddress = string.Empty;
        string[]? ipAddress;
        string[]? ipSubnet;
        string[]? dnsServers;
        string? dhcpServer;
        string? defaultGateway;
        bool dhcpEnabled = false;
        int networkSpeed = 0;

        public MainWindow()
        {
            InitializeComponent();
            GetSystemInfo();
            GetAllNetworkAdapters();
            GetActiveNetworkAdapters();
            //GetNetworkInfo();
            SendData();
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
                    manufacturer = queryObj["Manufacturer"]?.ToString();
                    model = queryObj["Model"]?.ToString();
                    totalPhysicalMemory = queryObj["TotalPhysicalMemory"]?.ToString();
                    computerName = queryObj["Name"]?.ToString();

                    // Afficher les informations dans la fenêtre WPF
                    systemInfoTextBox.Text += $"Manufacturer: {manufacturer}\n";
                    systemInfoTextBox.Text += $"Model: {model}\n";
                    systemInfoTextBox.Text += $"Total Physical Memory: {totalPhysicalMemory} bytes\n";
                    systemInfoTextBox.Text += $"Computer Name: {computerName}\n";
                }

                // Obtenir des informations sur le système d'exploitation
                ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject queryObj in osSearcher.Get())
                {
                    osVersion = queryObj["Caption"]?.ToString();
                    string? currentUser = queryObj["RegisteredUser"]?.ToString();
                    string freePhysicalMemory = queryObj["FreePhysicalMemory"]?.ToString();

                    // Afficher les informations sur le système d'exploitation
                    systemInfoTextBox.Text += $"OS Version: {osVersion}\n";
                    systemInfoTextBox.Text += $"Current User: {currentUser}\n";
                    systemInfoTextBox.Text += $"Free Physical Memory: {freePhysicalMemory} bytes\n";
                }
            }
            catch (ManagementException ex)
            {
                // Gérer les exceptions de requête WMI
                MessageBox.Show($"An error occurred while querying WMI: {ex.Message}");
            }
        }

        private void GetAllNetworkAdapters()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string description = queryObj["Description"]?.ToString() ?? "N/A";
                    string macAddress = queryObj["MACAddress"]?.ToString() ?? "N/A";
                    bool netEnabled = queryObj["NetEnabled"] != null && (bool)queryObj["NetEnabled"];
                    int netConnectionStatus = queryObj["NetConnectionStatus"] != null ? Convert.ToInt32(queryObj["NetConnectionStatus"]) : -1;

                    networkInfoTextBox.Text += $"Description: {description}\n";
                    networkInfoTextBox.Text += $"MAC Address: {macAddress}\n";
                    networkInfoTextBox.Text += $"Net Enabled: {netEnabled}\n";
                    networkInfoTextBox.Text += $"Net Connection Status: {netConnectionStatus}\n\n";
                }
            }
            catch (ManagementException ex)
            {
                MessageBox.Show($"An error occurred while querying network information: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
            }
        }


        private void GetActiveNetworkAdapters()
        {
            try
            {
                // Rechercher les adaptateurs réseau activés (NetEnabled = TRUE)
                ManagementObjectSearcher adapterSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = TRUE");
                foreach (ManagementObject adapter in adapterSearcher.Get())
                {
                    string description = adapter["Description"]?.ToString() ?? "N/A";
                    string macAddress = adapter["MACAddress"]?.ToString() ?? "N/A";
                    string adapterName = adapter["Name"]?.ToString() ?? "N/A";
                    string settingID = adapter["SettingID"]?.ToString() ?? "N/A";

                    // Afficher les informations de base de l'adaptateur
                    networkInfoTextBox.Text += $"Name: {adapterName}\n";
                    networkInfoTextBox.Text += $"Description: {description}\n";
                    networkInfoTextBox.Text += $"MAC Address: {macAddress}\n";

                    // Obtenir la configuration IP de l'adaptateur
                    string query = $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE SettingID = '{settingID}'";
                    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher(query);
                    foreach (ManagementObject config in configSearcher.Get())
                    {
                        string[] ipAddress = config["IPAddress"] as string[];
                        string[] ipSubnet = config["IPSubnet"] as string[];
                        string[] dnsServers = config["DNSServerSearchOrder"] as string[];
                        string dhcpServer = config["DHCPServer"]?.ToString() ?? "N/A";
                        string defaultGateway = (config["DefaultIPGateway"] as string[])?.FirstOrDefault() ?? "N/A";
                        bool dhcpEnabled = (bool)config["DHCPEnabled"];
                        int networkSpeed = (int)(config["Speed"] != null ? Convert.ToUInt64(config["Speed"]) : 0);

                        // Afficher les informations de configuration IP
                        networkInfoTextBox.Text += $"IP Address: {string.Join(", ", ipAddress ?? new string[] { "N/A" })}\n";
                        networkInfoTextBox.Text += $"IP Subnet: {string.Join(", ", ipSubnet ?? new string[] { "N/A" })}\n";
                        networkInfoTextBox.Text += $"DNS Servers: {string.Join(", ", dnsServers ?? new string[] { "N/A" })}\n";
                        networkInfoTextBox.Text += $"DHCP Server: {dhcpServer}\n";
                        networkInfoTextBox.Text += $"Default Gateway: {defaultGateway}\n";
                        networkInfoTextBox.Text += $"DHCP Enabled: {dhcpEnabled}\n";
                        networkInfoTextBox.Text += $"Speed: {networkSpeed} bps\n\n";
                    }
                }
            }
            catch (ManagementException ex)
            {
                MessageBox.Show($"An error occurred while querying network information: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    
    




        private async Task SendComputers()
        {
            computers.Id = 0;
            computers.RoomId = 1;
            computers.Caption = osVersion;
            computers.Manufacturer = manufacturer;
            computers.Model = model;
            computers.Name = computerName;
            computers.TotalPhysicalMemory = (int)Convert.ToInt64(totalPhysicalMemory);
            computers.Caption = osVersion;

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7227/api/Computers", computers);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Computer info sent successfully!");
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Failed to send computer info: {response.ReasonPhrase}\n{responseContent}");
            }
        }

        private async Task SendNetworkInfo()
        {
            networks.NetworkId = 3;
            networks.ComputerId = 1;

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7227/api/Networks", networks);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Network info sent successfully!");
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Failed to send network info: {response.ReasonPhrase}\n{responseContent}");
            }
        }

        private async void SendData()
        {
            await SendComputers();
            await SendNetworkInfo();
        }
    }
}