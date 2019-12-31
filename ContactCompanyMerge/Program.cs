using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace ContactCompanyMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            String contact_connectionString = "Data Source=EBAILEY;Initial Catalog=Contacts;Integrated Security=True";
            String eng_connectionString = "Data Source=EBAILEY;Initial Catalog=EngineeringTest;Integrated Security=True";


            List<BTR_Contact> BTR_ContactList = new List<BTR_Contact>();

            //BTR contact
            using (SqlConnection BTR_connection = new SqlConnection(contact_connectionString))
            {
                SqlCommand BTR_cmd = new SqlCommand("GetActiveBTRCompany", BTR_connection);
                BTR_cmd.CommandType = CommandType.StoredProcedure;

                BTR_connection.Open();

                SqlDataReader BTR_rdr = BTR_cmd.ExecuteReader();
                int id = 0;

                while (BTR_rdr.Read())
                {
                    BTR_Contact btr_contact = new BTR_Contact();
                    btr_contact.ID = id;
                    btr_contact.RefNum = BTR_rdr["Ref Nbr"].ToString();
                    btr_contact.Name = BTR_rdr["Business name"].ToString();
                    btr_contact.Formatted_Name = BTR_rdr["Formatted_Name_Company"].ToString();
                    Console.WriteLine(btr_contact.ID.ToString() + " | " + btr_contact.Name + " | " + btr_contact.Formatted_Name);
                    BTR_ContactList.Add(btr_contact);
                    id++;
                }
                BTR_connection.Close();
            }

            //Engineering Applicant Contact
            //List<ENG_Contact> ENG_AppContactList = new List<ENG_Contact>();

            List<ENG_Contact> ENG_ContactList = new List<ENG_Contact>();

            using (SqlConnection ENG_connection = new SqlConnection(eng_connectionString))
            {
                SqlCommand ENG_cmd = new SqlCommand("GetEngAppCompany", ENG_connection);
                ENG_cmd.CommandType = CommandType.StoredProcedure;
                ENG_connection.Open();

                SqlDataReader ENG_rdr = ENG_cmd.ExecuteReader();
                int appcontact_id = 0;

                while (ENG_rdr.Read())
                {
                    ENG_Contact eng_appcontact = new ENG_Contact();
                    eng_appcontact.ID = appcontact_id;
                    eng_appcontact.ProjectNo = ENG_rdr["ProjectNo"].ToString();
                    eng_appcontact.Name = ENG_rdr["AppCompany"].ToString();
                    eng_appcontact.Formatted_Name = ENG_rdr["Formatted_Name_app_company"].ToString();
                    Console.WriteLine(eng_appcontact.ProjectNo.ToString() + " | " + eng_appcontact.Name + " | " + eng_appcontact.Formatted_Name);
                    ENG_ContactList.Add(eng_appcontact);
                    appcontact_id++;
                }
                ENG_connection.Close();
            }

            //Engineering Owner Contact
            //List<ENG_Contact> ENG_OwnContactList = new List<ENG_Contact>();

            using (SqlConnection ENG_connection = new SqlConnection(eng_connectionString))
            {
                SqlCommand ENG_cmd = new SqlCommand("GetEngOwnCompany", ENG_connection);
                ENG_cmd.CommandType = CommandType.StoredProcedure;
                ENG_connection.Open();

                SqlDataReader ENG_rdr = ENG_cmd.ExecuteReader();
                int owncontact_id = 0;

                while (ENG_rdr.Read())
                {
                    ENG_Contact eng_owncontact = new ENG_Contact();
                    eng_owncontact.ID = owncontact_id;
                    eng_owncontact.ProjectNo = ENG_rdr["ProjectNo"].ToString();
                    eng_owncontact.Name = ENG_rdr["OwnCompany"].ToString();
                    eng_owncontact.Formatted_Name = ENG_rdr["Formatted_Name_own_company"].ToString();
                    Console.WriteLine(eng_owncontact.ProjectNo.ToString() + " | " + eng_owncontact.Name + " | " + eng_owncontact.Formatted_Name);
                    ENG_ContactList.Add(eng_owncontact);
                    owncontact_id++;
                }
                ENG_connection.Close();
            }

            ////Engineering Contractor Contact
            //List<ENG_Contact> ENG_ContContactList = new List<ENG_Contact>();

            using (SqlConnection ENG_connection = new SqlConnection(eng_connectionString))
            {
                SqlCommand ENG_cmd = new SqlCommand("GetEngContCompany", ENG_connection);
                ENG_cmd.CommandType = CommandType.StoredProcedure;
                ENG_connection.Open();

                SqlDataReader ENG_rdr = ENG_cmd.ExecuteReader();
                int contcontact_id = 0;

                while (ENG_rdr.Read())
                {
                    ENG_Contact eng_contcontact = new ENG_Contact();
                    eng_contcontact.ID = contcontact_id;
                    eng_contcontact.ProjectNo = ENG_rdr["ProjectNo"].ToString();
                    eng_contcontact.Name = ENG_rdr["ContCompany"].ToString();
                    eng_contcontact.Formatted_Name = ENG_rdr["Formatted_Name_cont_company"].ToString();
                    Console.WriteLine(eng_contcontact.ProjectNo.ToString() + " | " + eng_contcontact.Name + " | " + eng_contcontact.Formatted_Name);
                    ENG_ContactList.Add(eng_contcontact);
                    contcontact_id++;
                }
                ENG_connection.Close();
            }

            //Handle code contractors
            List<CODE_Cont_Contact> CODE_ContactList = new List<CODE_Cont_Contact>();

            using (SqlConnection CODE_connection = new SqlConnection(contact_connectionString))
            {
                SqlCommand CODE_cmd = new SqlCommand("GetActiveCODEContractorCompany", CODE_connection);
                CODE_cmd.CommandType = CommandType.StoredProcedure;

                CODE_connection.Open();

                SqlDataReader CODE_rdr = CODE_cmd.ExecuteReader();
                int id = 0;

                while (CODE_rdr.Read())
                {
                    CODE_Cont_Contact code_cont_contact = new CODE_Cont_Contact();
                    code_cont_contact.ID = id;
                    code_cont_contact.ContKey = CODE_rdr["CCONTKEY"].ToString();
                    code_cont_contact.Name = CODE_rdr["CCOMPNAME"].ToString();
                    code_cont_contact.Formatted_Name = CODE_rdr["Formatted_Name_Company"].ToString();
                    Console.WriteLine(code_cont_contact.ID.ToString() + " | " + code_cont_contact.Name + " | " + code_cont_contact.Formatted_Name);
                    CODE_ContactList.Add(code_cont_contact);
                    id++;
                }
                CODE_connection.Close();
            }

            //LOAD BTR INTO DESTINATION TABLE
            List<Destination> DestinationTable = new List<Destination>();

            //for (int i = 0; i < BTR_ContactList.Count; i++)
            while (BTR_ContactList.Count > 0)
            {
                BTR_Contact cur_contact = BTR_ContactList.First();
                BTR_ContactList.Remove(cur_contact);

                List<string> btr_ids = new List<string>();
                btr_ids.Add(cur_contact.RefNum);

                for (int j = 0; j < BTR_ContactList.Count; j++)
                {
                    if (cur_contact.Formatted_Name == BTR_ContactList[j].Formatted_Name)
                    {
                        btr_ids.Add(BTR_ContactList[j].RefNum);
                        Console.WriteLine("Added BTR ID of " + BTR_ContactList[j].RefNum);
                        BTR_ContactList.RemoveAt(j);
                    }
                }

                Destination destination = new Destination();
                destination.Name = cur_contact.Name;
                destination.Name_Formatted = cur_contact.Formatted_Name;
                destination.BTR_IDs = ProcessIDs(btr_ids);
                destination.ENG_IDs = "";
                destination.CODE_Cont_IDs = "";
                DestinationTable.Add(destination);
                Console.WriteLine("BTR Record written to Destination with name: " + destination.Name);

            }

            //Reduce engineering list and match across destination table
            List<Destination> Reduced_Eng_Contacts = new List<Destination>();

            //for(int i = 0; i < ENG_ContactList.Count; i++)
            while (ENG_ContactList.Count > 0)
            {
                ENG_Contact cur_contact = ENG_ContactList.First();
                ENG_ContactList.Remove(cur_contact);

                List<string> eng_ids = new List<string>();
                eng_ids.Add(cur_contact.ProjectNo);

                for (int j = 0; j < ENG_ContactList.Count; j++)
                {
                    if (cur_contact.Formatted_Name == ENG_ContactList[j].Formatted_Name)
                    {
                        eng_ids.Add(ENG_ContactList[j].ProjectNo);
                        Console.WriteLine("Added ENGApp ID of " + ENG_ContactList[j].ProjectNo);
                        ENG_ContactList.RemoveAt(j);
                    }
                }

                Destination destination = new Destination
                {
                    Name = cur_contact.Name,
                    Name_Formatted = cur_contact.Formatted_Name,
                    BTR_IDs = "",
                    ENG_IDs = ProcessIDs(eng_ids),
                    CODE_Cont_IDs = ""
                };
                Reduced_Eng_Contacts.Add(destination);
                Console.WriteLine("ENG Record written to Destination with id name: " + destination.Name);
            }

            for (int i = 0; i < Reduced_Eng_Contacts.Count; i++)
            //while(Reduced_Eng_Contacts.Count > 0)
            {
                Destination cur_eng = Reduced_Eng_Contacts[i];


                for (int j = 0; j < DestinationTable.Count; j++)
                {
                    if (cur_eng.Name_Formatted == DestinationTable[j].Name_Formatted)
                    {

                        DestinationTable[j].ENG_IDs = cur_eng.ENG_IDs;
                        Reduced_Eng_Contacts.Remove(cur_eng);
                        break;
                    }
                }
            }

            for (int i = 0; i < Reduced_Eng_Contacts.Count; i++)
            {
                Destination destination = new Destination
                {
                    Name = Reduced_Eng_Contacts[i].Name,
                    Name_Formatted = Reduced_Eng_Contacts[i].Name_Formatted,
                    BTR_IDs = "",
                    ENG_IDs = Reduced_Eng_Contacts[i].ENG_IDs,
                    CODE_Cont_IDs = ""
                };
                Console.WriteLine("Eng written to Destination with id name:" + destination.Name);
                DestinationTable.Add(destination);

            }


            //Reduce Code Contractors

            List<Destination> Reduced_Code_Contacts = new List<Destination>();

            //for (int i = 0; i < CODE_ContactList.Count; i++)
            while (CODE_ContactList.Count > 0)
            {
                CODE_Cont_Contact cur_contact = CODE_ContactList.First();
                CODE_ContactList.Remove(cur_contact);

                List<string> code_ids = new List<string>();
                code_ids.Add(cur_contact.ContKey);

                for (int j = 0; j < ENG_ContactList.Count; j++)
                {
                    if (cur_contact.Formatted_Name == CODE_ContactList[j].Formatted_Name)
                    {
                        code_ids.Add(CODE_ContactList[j].ContKey);
                        Console.WriteLine("Added CODEApp ID of " + CODE_ContactList[j].ContKey);
                        CODE_ContactList.RemoveAt(j);
                    }
                }

                Destination destination = new Destination
                {
                    Name = cur_contact.Name,
                    Name_Formatted = cur_contact.Formatted_Name,
                    BTR_IDs = "",
                    ENG_IDs = "",
                    CODE_Cont_IDs = ProcessIDs(code_ids),
                };
                Reduced_Code_Contacts.Add(destination);
                Console.WriteLine("CODE Record written to Destination with name:  " + destination.Name);
            }


            for (int i = 0; i < Reduced_Code_Contacts.Count; i++)
            //while(Reduced_Code_Contacts.Count > 0)
            {
                Destination cur_code = Reduced_Code_Contacts[i];


                for (int j = 0; j < DestinationTable.Count; j++)
                {
                    if (cur_code.Name_Formatted == DestinationTable[j].Name_Formatted)
                    {

                        DestinationTable[j].CODE_Cont_IDs = cur_code.CODE_Cont_IDs;
                        Reduced_Code_Contacts.Remove(cur_code);
                        break;
                    }
                }
            }

            for (int i = 0; i < Reduced_Code_Contacts.Count; i++)
            {
                Destination destination = new Destination
                {
                    Name = Reduced_Code_Contacts[i].Name,
                    Name_Formatted = Reduced_Code_Contacts[i].Name_Formatted,
                    BTR_IDs = "",
                    ENG_IDs = "",
                    CODE_Cont_IDs = Reduced_Code_Contacts[i].CODE_Cont_IDs
                };
                Console.WriteLine("Code written to Destination with name: " + destination.Name);
                DestinationTable.Add(destination);
            }

            //Move DestinationTable object to SQL using stored procedure
            for (int i = 0; i < DestinationTable.Count; i++)
            {
                if (AddCompany(DestinationTable[i].Name, DestinationTable[i].Name_Formatted, DestinationTable[i].BTR_IDs, DestinationTable[i].ENG_IDs, DestinationTable[i].CODE_Cont_IDs))
                {
                    Console.WriteLine("Sucessfully added record with name " + DestinationTable[i].Name + " and ENG_IDS: " + DestinationTable[i].ENG_IDs);
                }
                else
                {
                    Console.WriteLine("Record insert FAILED with name " + DestinationTable[i].Name);
                }
            }
        }

        static string MakeIDUnique(string input)
        {
            string[] input_split;
            input_split = input.Split(";");
            input_split.Distinct().ToArray();
            string output = "";

            for (int i = 0; i < input_split.Length; i++)
            {
                output = output + input_split[i] + ";";
            }

            return output;
        }

        static string ProcessIDs(List<string> ids)
        {
            string output = "";

            List<string> distinct_list = ids.Distinct().ToList<string>();

            for (int i = 0; i < distinct_list.Count(); i++)
            {
                output = output + distinct_list[i] + ";";
            }

            //output = output.Replace(";;", ";");
            Console.WriteLine(output);
            return output;
        }

        static bool AddContact(string name, string name_formatted, string btr_ids, string eng_ids, string code_cont_ids)
        {
            String contact_connectionString = "Data Source=EBAILEY;Initial Catalog=Contacts;Integrated Security=True";

            using (SqlConnection Insert_connection = new SqlConnection(contact_connectionString))
            {
                SqlCommand Insert_cmd = new SqlCommand("InsertIndividualContact", Insert_connection);
                Insert_cmd.CommandType = CommandType.StoredProcedure;
                Insert_cmd.Parameters.AddWithValue("@name", name);
                Insert_cmd.Parameters.AddWithValue("@name_formatted", name_formatted);
                Insert_cmd.Parameters.AddWithValue("@BTR_IDs", btr_ids);
                Insert_cmd.Parameters.AddWithValue("@ENG_IDs", eng_ids);
                Insert_cmd.Parameters.AddWithValue("@CODE_Cont_IDs", code_cont_ids);

                Insert_connection.Open();

                int i = Insert_cmd.ExecuteNonQuery();

                Insert_connection.Close();

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        static bool AddCompany(string name, string name_formatted, string btr_ids, string eng_ids, string code_cont_ids)
        {
            String contact_connectionString = "Data Source=EBAILEY;Initial Catalog=Contacts;Integrated Security=True";

            using (SqlConnection Insert_connection = new SqlConnection(contact_connectionString))
            {
                SqlCommand Insert_cmd = new SqlCommand("InsertIndividualCompany", Insert_connection);
                Insert_cmd.CommandType = CommandType.StoredProcedure;
                Insert_cmd.Parameters.AddWithValue("@name", name);
                Insert_cmd.Parameters.AddWithValue("@name_formatted", name_formatted);
                Insert_cmd.Parameters.AddWithValue("@BTR_IDs", btr_ids);
                Insert_cmd.Parameters.AddWithValue("@ENG_IDs", eng_ids);
                Insert_cmd.Parameters.AddWithValue("@CODE_Cont_IDs", code_cont_ids);

                Insert_connection.Open();

                int i = Insert_cmd.ExecuteNonQuery();

                Insert_connection.Close();

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

