using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PROJET_BDD
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("RequeteMobile.xml");
            bool flagID = false;
            string ID = "";
            string arrondissement = "";

            #region Lancement SQL
            string connectionString = "SERVER=fboisson.ddns.net;PORT=3306;DATABASE=shen_alex;UID=S6-SHEN-ALEX;PASSWORD=211594;encrypt=false";
            //string connectionString = "SERVER=localhost;PORT=3306;DATABASE=shen_alex;UID=Alexandre;PASSWORD=abcd1234;encrypt=false";

            MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                        
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * from client";

            MySqlDataReader reader;

            /*
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0) + reader.GetString(1) + reader.GetString(2) + reader.GetString(3) + reader.GetString(4));
            }
            */
            #endregion

            #region Variables
            string nom = "";
            string adresse = "";
            string noTel = "";
            string email = "";

            string immat = "";
            string marque = "";
            string modele = "";
            string nomParking = "";
            string idPlace = "";
            string nomControleur = "";
            #endregion

            #region Lecture XML Client
            foreach (XmlNode childNode in doc.DocumentElement.ChildNodes)
            {
                if (childNode.Name == "numeroInscription")
                {
                    ID = childNode.InnerText;
                    if (ID != "" || ID != null)
                    {
                        flagID = true;
                    }
                }
                else if (childNode.Name == "arrondissement")
                {
                    arrondissement = childNode.InnerText;
                }
            }
            if (!flagID)
            {
                foreach (XmlNode childNode in doc.DocumentElement.ChildNodes)
                {
                    if (childNode.Name == "nom")
                    {
                        nom = childNode.InnerText;
                    }
                    else if (childNode.Name == "adresse")
                    {
                        adresse = childNode.InnerText;
                    }
                    else if (childNode.Name == "numeroDeTel")
                    {
                        noTel = childNode.InnerText;
                    }
                    else if (childNode.Name == "email")
                    {
                        email = childNode.InnerText;
                    }
                    
                }
                //reader.Close();
                command.CommandText = "SELECT count(idClient) from client;";
                reader = command.ExecuteReader();
                reader.Read();
                int nbClients = Convert.ToInt32(reader.GetString(0));
                nbClients++;
                string nbClientsString = Convert.ToString(nbClients);
                reader.Close();
                command.CommandText = "INSERT INTO shen_alex.client (idClient, nom, adresse, email, tel) VALUES ('"+nbClientsString+"', '"+nom+"', '"+adresse+"', '"+email+"', '"+noTel+"');" ;
                reader = command.ExecuteReader();
                try
                {
                    reader.Read();
                    Console.WriteLine("Nouveau client ajouté: "+nbClientsString+", "+nom+", "+adresse+", "+email+", "+noTel);
                    reader.Close();
                }
                catch
                {
                    Console.WriteLine("Erreur d'écriture dans la base de données.");
                }
                ID = nbClientsString;
            }
            #endregion

            #region Recherche d'un véhicule

            Console.WriteLine();
            Console.WriteLine("Sélection d'une voiture dans l'arrondissement " + arrondissement + "...");
            string villeArron = "750" + arrondissement;
            command.CommandText = "select immat, marque, modele, nomParking, idPlace, nomControleur from vehicule where nomParking in (select nomParking from Parking where Parking.nomParking = vehicule.nomparking and arrondissement ="+villeArron+") and fonctionnel = true;";
            reader = command.ExecuteReader();
            //reader.Read();
            

            if(reader.Read())
            {
                immat = reader.GetString(0);
                marque = reader.GetString(1);
                modele = reader.GetString(2);
                nomParking = reader.GetString(3);
                idPlace = reader.GetString(4);
                nomControleur = reader.GetString(5);
            }

            if(immat!="" && immat!=null)
            {
                Console.WriteLine("Véhicule disponible! " + immat + ", " + marque + ", " + modele);
            }
            else
            {
               
                int arrond = Convert.ToInt32(villeArron);
                arrond++;
                
                while(immat=="")
                {
                    while (arrond<=75020&&arrond>=75001)
                    {
                        Console.WriteLine("Pas de véhicule trouvé dans le " + (arrond - 1) + "ème arrondissement. Elargissement de la recherche");
                        reader.Close();
                        string arrondString = Convert.ToString(arrond);
                        command.CommandText = "select immat, marque, modele, nomParking, idPlace, nomControleur from vehicule where nomParking in (select nomParking from Parking where Parking.nomParking = vehicule.nomparking and arrondissement =" + arrondString + ") and fonctionnel = true;";
                        reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            immat = reader.GetString(0);
                            marque = reader.GetString(1);
                            modele = reader.GetString(2);
                            nomParking = reader.GetString(3);
                            idPlace = reader.GetString(4);
                            nomControleur = reader.GetString(5);
                        }
                        Console.WriteLine("Sélection d'une voiture dans l'arrondissement " + arrondString + "...");
                        if (immat != "" && immat != null)
                        {
                            Console.WriteLine("Véhicule disponible! " + immat + ", " + marque + ", " + modele);
                            break;
                        }
                        arrond++;
                    }
                    if (immat == "" ^ immat == null)
                    {
                       
                        arrond = Convert.ToInt32(villeArron);
                        while (arrond >= 075001 && arrond<=75020)
                        {
                            Console.WriteLine("Pas de véhicule trouvé dans le " + (arrond) + "ème arrondissement. Elargissement de la recherche");
                            string arrondString = Convert.ToString(arrond);
                            
                            command.CommandText = "select immat, marque, modele, nomParking, idPlace, nomControleur from vehicule where nomParking in (select nomParking from Parking where Parking.nomParking = vehicule.nomparking and arrondissement =" + arrondString + ") and fonctionnel = true;";
                            reader.Close();
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                immat = reader.GetString(0);
                                marque = reader.GetString(1);
                                modele = reader.GetString(2);
                                nomParking = reader.GetString(3);
                                idPlace = reader.GetString(4);
                                nomControleur = reader.GetString(5);
                            }
                            Console.WriteLine("Sélection d'une voiture dans l'arrondissement " + arrondString + "...");
                            if (immat != "" && immat != null)
                            {
                                Console.WriteLine("Véhicule disponible! " + immat + ", " + marque + ", " + modele);

                                break;
                            }
                            arrond--;
                        }
                    }
                    break;

                }
            }
            reader.Close();
            Console.WriteLine();
            #endregion

            #region Lecture du JSON RBNP

            Console.WriteLine("Réception des informations depuis RBNP...");

            
            List<Accomodation> items = new List<Accomodation>();
            using (StreamReader r = new StreamReader("RBNP.json"))
            {
                string json = r.ReadToEnd();
                string good = json.Replace("\ufeff", "");
                items = JsonConvert.DeserializeObject<List<Accomodation>>(good);
            }

            
            IEnumerable<Accomodation> listResult = items.Where(el => el.borough == arrondissement && el.overall_satisfaction>=4.5 && el.availability=="yes");
            List< Accomodation > listeFinale = new List<Accomodation>();
            if(listResult.Count()>=3)
            {
                for(int i=0; i<3; i++)
                {
                    var appart = listResult.ElementAt(i);
                    listeFinale.Add(appart);
                }
            }
            else
            {
                for(int i=0; i<listResult.Count(); i++)
                {
                    var appart = listResult.ElementAt(i);
                    listeFinale.Add(appart);
                }
            }
            foreach(Accomodation acc in listeFinale)
            {
                Console.WriteLine("Quartier: " + acc.neighborhood + " Id hote:  " + acc.host_id + " Id logement: " + acc.room_id + " Prix: " + acc.price + " euros Note moyenne: " + acc.overall_satisfaction + " Disponibilité: " + acc.availability);
            }

            #endregion


            #region Choix de l'appartement par le client
            if (listeFinale.Count==0)
            {
                Console.WriteLine("Aucun appartement disponible.");
            }
            else
            {
                int nbApparts = listeFinale.Count();

                #region Génération fichier XML avec les trois appartements sélectionnés
                //XmlDocument troisSejours = new XmlDocument();
                //XmlWriter troisSejours = XmlWriter.Create();
                command.CommandText = "select MAX(idSejour) from sejour";
                reader = command.ExecuteReader();
                string idSejour = "";
                if (reader.Read())
                {
                    idSejour =System.Convert.ToString(System.Convert.ToInt32( reader.GetString(0))+1);
                }
                reader.Close();
                command.CommandText="select idTheme from theme where arrondissement="+arrondissement+";";
                reader = command.ExecuteReader();
                string idTheme = "";
                if (reader.Read())
                {
                    idTheme = reader.GetString(0);
                }
                reader.Close();
                XDocument troisSejours = XDocument.Load("ValidationSejour.xml");
                XElement Sejour = troisSejours.Element("Sejour");
                object[] appart1 = new object[2];
                appart1[0] = listeFinale[0].room_id;
                appart1[1] = listeFinale[0].price;
                object[] appart2 = new object[2];
                appart2[0] = listeFinale[1].room_id;
                appart2[1] = listeFinale[1].price;
                object[] appart3 = new object[2];
                appart3[0] = listeFinale[2].room_id;
                appart3[1] = listeFinale[2].price;
                Sejour.Add(new XElement("numeroSejour",idSejour), // A REMPLIR
                           new XElement("nom", nom), 
                           new XElement("numeroClient", ID),
                           new XElement("theme",idTheme), //A REMPLIR
                           new XElement("semaine", 14),
                           new XElement("nomParking", nomParking),
                           new XElement("numeroPlaceParking", idPlace),
                           new XElement("immatVoiture", immat),
                           new XElement("arrondissement", arrondissement),
                           new XElement("appartement", appart1),
                           new XElement("appartement", appart2),
                           new XElement("appartement", appart3)
                           );
                /*
                XElement Appart1 = troisSejours.Element("appartement");
                Appart1.Add(
                    new XElement("room_id", listeFinale[0].room_id),
                    new XElement("prix", listeFinale[0].price)
                    );
                XElement Appart2 = troisSejours.Element("appartement");
                Appart2.Add(
                    new XElement("room_id", listeFinale[1].room_id),
                    new XElement("prix", listeFinale[1].price)
                    );
                XElement Appart3 = troisSejours.Element("appartement");
                Appart3.Add(
                    new XElement("room_id", listeFinale[2].room_id),
                    new XElement("prix", listeFinale[3].price)
                    );
                    */
                troisSejours.Save("ValidationSejour.xml");


                #endregion


                Random choixClientAppart = new Random();
                int choix = choixClientAppart.Next(0, nbApparts);
                Console.WriteLine();
                Console.WriteLine("Appartement choisi par le client: " + (choix+1));
                List<Accomodation> temp = new List<Accomodation>();
                temp.Add(listeFinale[choix]);
                XDocument validClient = XDocument.Load("ValidationSejourClient.xml");
                XElement sejour = validClient.Element("Sejour");
                sejour.Add(new XElement("numeroSejour", idSejour), // A REMPLIR
                           new XElement("numeroClient", ID),
                           new XElement("validationSejour", 1), //A REMPLIR
                           new XElement("numAppart", temp[0].room_id)
                           );
                validClient.Save("ValidationSejourClient.xml");

                command.CommandText = "insert into shen_alex.sejour (idSejour, idTheme, immat, idHebergement, noteClient, avisClient,idClient,semaine) values (" + idSejour + ", '" + idTheme + "', '"+immat+"', '"+temp[0].room_id+"', '0','0','"+ID+"','14');";
                reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
                listeFinale = temp;


                #region Génération du fichier XML avec l'appartement choisi par le client


                #endregion
            }

            
            //Console.WriteLine("Hello world");
            listeFinale[0].availability = "no";
            #endregion

            #region Génération du format JSON
            string jsonData = JsonConvert.SerializeObject(listeFinale[0]);
            jsonData = jsonData.Insert(0, "[");
            jsonData = jsonData.Insert(jsonData.Length, "]");

            
            Console.WriteLine(jsonData);
            
            Console.WriteLine();
            #endregion

            #region Requêtes SQL
            //Le client a déposé sa voiture à la même place que là où il l'a prise.
            command.CommandText = "update vehicule set idPlace='"+idPlace+"' where immat='"+immat+"';";
            reader = command.ExecuteReader();
            reader.Read();
            reader.Close();
            Console.WriteLine("Voiture rendue sur la place " + idPlace + ".");
            
            Console.WriteLine();

            //Contrôle de la voiture par un vérificateur
            Console.WriteLine("Le vérificateur contrôle la voiture.");
            Console.WriteLine();


            //Le véhicule doit être nettoyé
            command.CommandText = "update vehicule set fonctionnel='0', motifInfonctionnel='Nettoyage nécessaire' where immat='" + immat + "';";
            reader = command.ExecuteReader();
            reader.Read();
            reader.Close();
            command.CommandText = "select motifInfonctionnel from vehicule where immat='"+immat+"';";
            reader = command.ExecuteReader();
            string message = "";
            if(reader.Read())
            {
                message = reader.GetString(0);
                Console.WriteLine("Constat du vérificateur: "+ message); 
            }
            reader.Close();
            command.CommandText = "select count(*) from intervention;";
            reader = command.ExecuteReader();
            int noIntervention = 0;
            if(reader.Read())
            {
                noIntervention = Convert.ToInt32(reader.GetString(0))+1;
            }
            reader.Close();
            command.CommandText = "insert into shen_alex.intervention (idIntervention, nomControleur, descIntervention, dateIntervention, immat) values ("+noIntervention+", '"+nomControleur+"', '"+message+"', '2018-05-13', '"+immat+"');";
            reader = command.ExecuteReader();
            reader.Read();
            reader.Close();
            
            Console.WriteLine();


            //Nettoyage du véhicule
            Console.WriteLine("Nettoyage du véhicule...");
            
            Console.WriteLine();


            //Remise en service de la voiture
            command.CommandText = "update vehicule set fonctionnel='1', motifInfonctionnel='' where immat='" + immat + "';";
            reader = command.ExecuteReader();
            reader.Read();
            reader.Close();
            Console.WriteLine("Véhicule remis en service.");
            command.CommandText = "select count(*) from intervention;";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                noIntervention = Convert.ToInt32(reader.GetString(0)) + 1;
            }
            reader.Close();
            command.CommandText = "insert into shen_alex.intervention (idIntervention, nomControleur, descIntervention, dateIntervention, immat) values (" + noIntervention + ", '" + nomControleur + "', 'Remise en service', '2018-05-13', '" + immat + "');";
            reader = command.ExecuteReader();
            reader.Read();
            reader.Close();
            
            Console.WriteLine();


            //Historique des interventions sur un véhicule
            Console.WriteLine("Interventions sur le véhicule " + immat + ":");
            command.CommandText = "select * from intervention where immat='"+immat+"';";
            List<string> interventions = new List<string>();
            reader = command.ExecuteReader();
            while(reader.Read())
            {
                string idInt = reader.GetString(0);
                string nomCont = reader.GetString(1);
                string descInt = reader.GetString(2);
                string dateInt = reader.GetString(3);
                string immattemp = reader.GetString(4);
                string all = idInt + " " + nomCont + " " + descInt + " " + dateInt + " " + immattemp;
                Console.WriteLine(all);
            }
            reader.Close();
            Console.WriteLine();

            //Nombre de locations du véhicule
            Console.WriteLine("Nombre de locations du véhicule " + immat + ":");
            command.CommandText = "select count(*) from intervention where immat='" + immat + "' and descIntervention='Remise en service';";
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine(reader.GetString(0));
            reader.Close();
            Console.WriteLine();

            //Historique d'un client
            Console.WriteLine("Historique du client " + ID + ":");
            command.CommandText = "select idSejour, semaine, idTheme, noteClient, idHebergement, immat from sejour where idClient='" + ID + "';";
            reader = command.ExecuteReader();
            string idHebergement = "";
            while(reader.Read())
            {
                Console.WriteLine("Client: " + ID + " Identifiant Séjour: " + reader.GetString(0) + " date: " + reader.GetString(1) + " Identifiant thème: " 
                                    + reader.GetString(2) + " Note accordée: " + reader.GetString(3) + " Immatriculation du véhicule: " + reader.GetString(5) + " Identifiant hébergement: " + reader.GetString(4));
            }
            reader.Close();

            command.CommandText = "select arrondissement from hebergement where idHebergement='" + idHebergement + "';";
            reader = command.ExecuteReader();
            if(reader.Read())
            {
                Console.Write(" Arrondissement de l'hébergement: " + reader.GetString(0));
            }
            reader.Close();

            //


            command.CommandText = "select idTheme,count(*) AS nombre from sejour group by idTheme order by nombre DESC;";
            List<string> idThemeDesc = new List<string>();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Le theme numero "+reader.GetString(0)+" est le theme le plus apprecié par les clients.");
                break;
            }
            reader.Close();
            Console.WriteLine();
            #endregion

            Console.ReadKey();
        }


    }
}
