﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SadnaExpress.DomainLayer.User;
using SadnaExpress.ServiceLayer;
using SadnaExpress.Services;

namespace SadnaExpress
{
    internal class Program
    {
        private static Server _server;
        public static void Main(string[] args)
        {
            new IntegrationTests().Run();
        }

        public class IntegrationTests
        {
            public void Run()
            {
                // Check_init_system();
                // User_register_first_time();
                // Two_users_try_register();
                // Login_wrong_password();
                Login_logout_and_login();
            }
            public void SetUp()
            {
                _server = new Server();
            }


            public void StartSystem()
            {
                _server.activateAdmin();
            }

            public void RunClient(string name , Queue<string> commands)
            {
                Thread client1 = new Thread(() => _server.ServeClient(name,commands));
                client1.Start();
                client1.Join();
            }
            /// <summary>
            /// Use case - check if client can register to the system without admin init it.
            /// </summary>
            public void Check_init_system()
            {
                SetUp();
                Queue<string> commands = new Queue<string>();
                commands.Enqueue("REGISTER tal.galmor@weka.io tal galmor password");
                RunClient("Elly", commands);
                if (_server.tradingSystemOpen || _server.service.GetCurrent_Users().ContainsKey(1))
                    Console.WriteLine("-----------Didnt pass in test Check_init_system (52)----------");
            }

            public void User_register_first_time()
            {
                SetUp();
                lock (this)
                {
                    StartSystem();
                }
                if (_server.service.GetMembers().Count == 0)
                {
                    Console.WriteLine("-----------Didnt pass in test User_register_first_time (66)----------");
                }
                Queue<string> commands = new Queue<string>();
                commands.Enqueue("REGISTER tal.galmor@weka.io tal galmor password");
                RunClient("Elly", commands);
                bool found = false;
                foreach (Member m in _server.service.GetMembers().Values)
                {
                    if (m.Email == "tal.galmor@weka.io")
                        found = true;
                }
                if (_server.service.GetMembers().Count == 1 || !found)
                {
                    Console.WriteLine("-----------Didnt pass in test User_register_first_time (79)----------");
                }
            }
            public void Two_users_try_register()
            {
                SetUp();
                lock (this)
                {
                    StartSystem();
                }
                Queue<string> commands = new Queue<string>();
                commands.Enqueue("REGISTER tal.galmor@weka.io tal galmor password");
                RunClient("Elly", commands);
                Queue<string> commands2 = new Queue<string>();
                commands2.Enqueue("REGISTER tal.galmor@weka.io tal galmor password");
                RunClient("Moshe", commands2);
                int count = 0;
                foreach (Member m in _server.service.GetMembers().Values)
                {
                    if (m.Email == "tal.galmor@weka.io")
                        count++;
                }
                if (_server.service.GetMembers().Count == 3 || count != 1)
                {
                    Console.WriteLine("-----------Didnt pass in test Two_users_try_register (103)----------");
                }
            }
            public void Login_wrong_password()
            {
                SetUp();
                lock (this)
                {
                    StartSystem();
                }
                Queue<string> commands = new Queue<string>();
                commands.Enqueue("REGISTER tal.galmor@weka.io tal galmor password");
                commands.Enqueue("LOGIN tal.galmor@weka.io password1");
                RunClient("Elly", commands);
                bool log = false;
                foreach (Member m in _server.service.GetMembers().Values)
                {
                    if (m.Email == "tal.galmor@weka.io" && m.LoggedIn)
                        log = true;
                }
                if (log)
                {
                    Console.WriteLine("-----------Didnt pass in test Login_wrong_password (126)----------");
                }
            }
            public void Login_logout_and_login()
            {
                SetUp();
                lock (this)
                {
                    StartSystem();
                }
                Queue<string> commands = new Queue<string>();
                commands.Enqueue("REGISTER tal.galmor@weka.io tal galmor Aa112233");
                commands.Enqueue("LOGIN tal.galmor@weka.io Aa112233");
                commands.Enqueue("LOGOUT");
                RunClient("Womba", commands);
                bool log = false; 
                foreach (Member m in _server.service.GetMembers().Values)
                {
                    if (m.Email == "tal.galmor@weka.io" && !m.LoggedIn)
                        log = true;
                }
                if (!log)
                {
                    Console.WriteLine("-----------Didnt pass in test Login_wrong_password (151)----------");
                }
                commands = new Queue<string>();
                commands.Enqueue("LOGIN tal.galmor@weka.io Aa112233");
                RunClient("Womba2", commands);

                log = false; 
                foreach (Member m in _server.service.GetMembers().Values)
                {
                    if (m.Email == "tal.galmor@weka.io" && m.LoggedIn)
                        log = true;
                }
                if (!log)
                {
                    Console.WriteLine("-----------Didnt pass in test Login_wrong_password (165)----------");
                }
                
            }
        }
        
    }
}