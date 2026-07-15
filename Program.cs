class Program
{
    const int OBJECTS_PER_PAGE = 4;
    const ConsoleKey NEXT_PAGE = ConsoleKey.RightArrow;
    const ConsoleKey PREV_PAGE = ConsoleKey.LeftArrow;
    const ConsoleKey EXIT_PAGES = ConsoleKey.X;
    const int DATA_LENGTH = 1000;
    static string[] SEX = new string[] { "Hombre", "Mujer" };
    static string[] APPOINTMENT_TYPE = new string[] { "General", "Especializada" };
    static string[] BLOOD_TYPE = new string[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

    // Pacientes
    static int[] patientFile = new int[DATA_LENGTH];
    static string[] patientName = new string[DATA_LENGTH];
    static int[] patientAge = new int[DATA_LENGTH];
    static string[] patientSex = new string[DATA_LENGTH];
    static string[] patientBloodType = new string[DATA_LENGTH];
    static int[] patientPhone = new int[DATA_LENGTH];
    static int patientCount = 0;

    // Medicos
    static int[] medicCode = new int[DATA_LENGTH];
    static string[] medicName = new string[DATA_LENGTH];
    static string[] medicSpecialty = new string[DATA_LENGTH];
    static int[] medicExperience = new int[DATA_LENGTH];
    static string[] medicSchedule = new string[DATA_LENGTH];
    static int medicCount = 0;

    // Consultorios
    static int[] officeNumber = new int[DATA_LENGTH];
    static int[] officeFloor = new int[DATA_LENGTH];
    static int[] officeCapacity = new int[DATA_LENGTH];
    static int[] officeQueue = new int[DATA_LENGTH];
    static bool[] officeAvailable = new bool[DATA_LENGTH];
    static int[] officeSpots = new int[DATA_LENGTH];
    static int officeCount = 0;

    // Citas
    static int[] appointmentNumber = new int[DATA_LENGTH];
    static string[] appointmentPatient = new string[DATA_LENGTH];
    static string[] appointmentMedic = new string[DATA_LENGTH];
    static string[] appointmentDate = new string[DATA_LENGTH];
    static string[] appointmentTime = new string[DATA_LENGTH];
    static string[] appointmentType = new string[DATA_LENGTH];
    static int appointmentCount = 0;

    // Especialidades
    static string[] specialtyName = new string[DATA_LENGTH];
    static int[] specialtyMedicCount = new int[DATA_LENGTH];
    static int[] specialtyOfficeAvailability = new int[DATA_LENGTH];
    static string[] specialtyInCharge = new string[DATA_LENGTH];
    static int specialtyCount = 0;

    static string BoolToString(bool value)
    {
        if (value)
        {
            return "Sí";
        } else
        {
            return "No";
        }
    }

    static bool ExistsInArrayIgnoreCase(string[] objects, string value)
    {   
        value = value.ToLower();

        for (int i = 0; i < objects.Length; i++)
        {
            string found = objects[i];
            if (string.IsNullOrEmpty(found)) continue;

            if (found.ToLower() == value)
            {
                return true;
            }
        }

        return false;
    }

    static int FetchInArray<T>(T[] objects, T value)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            T found = objects[i];
            if (found == null) continue;
            if (found.Equals(value))
            {
                return i;
            }
        }

        return -1;
    }

    static void AwaitKey()
    {
        Console.Write("\n\tPRESIONE PARA CONTINUAR...");
        Console.ReadKey();
    }
    
    static void AwaitKey(string message)
    {
        Console.Write("\n\t" + message.ToUpper());
        Console.ReadKey();
    }

    static string GetInput(string message)
    {
        Console.Write($"\t - {message}: ");
        string input = Console.ReadLine() ?? string.Empty;
        Console.Write("\n");
        return input;
    }

    static int GetIntInput(string message)
    {
        int output;
        bool valid;

        do {
            string input = GetInput(message);
            valid = int.TryParse(input, out output);

            if (!valid)
            {
                AwaitKey("POR FAVOR DIGITE UN NÚMERO ENTERO VALIDO. PRESIONE PARA CONTINUAR...");
                Console.WriteLine("\n\n");
            }
        } while (!valid);

        return output;
    }

    static string GetOptionInput(string message, string[] options)
    {
        string input;
        bool valid;

        message += " (";
        for (int i = 0; i < options.Length; i++)
        {
            string option = options[i];
            if (string.IsNullOrEmpty(option)) continue;

            if (i > 0)
            {
                message += "/";
            }

            message += option;
        }
        message += ")";

        do
        {
            input = GetInput(message);
            valid = ExistsInArrayIgnoreCase(options, input);

            if (!valid)
            {
                AwaitKey("POR FAVOR ESCRIBA UNA OPCIÓN VALIDA. PRESIONE PARA CONTINUAR...");
                Console.WriteLine("\n\n");
            }
        } while (!valid);

        return input;
    }

    static bool GetBoolInput(string message)
    {
        bool output = false;
        bool valid = false;

        message += " (S/n)";

        do
        {
            string input = GetInput(message).ToLower();
            if (input == "n" || input == "no")
            {
                output = false;
                valid = true;
            } else if (input == string.Empty || input == "s" || input == "si")
            {
                output = true;
                valid = true;
            }

        } while (!valid);

        return output;
    }

    static void GetSelection(string[] options, out int selected, out bool valid)
    {
        for (int i = 0; i < options.Length; i++)
        {
            string option = options[i];
            Console.WriteLine($"\t{i + 1}. {option}");
        }

        Console.WriteLine();
        string input = GetInput("Seleccione una opción");

        if (!int.TryParse(input, out selected) || selected < 1 || selected > options.Length)
        {
            selected = -1;
            valid = false;

            AwaitKey("ESCOJA UNA OPCION VALIDA, PRESIONE PARA CONTINUAR...");
        }
        else
        {
            valid = true;
        }
    }

    static void Title(string[] title)
    {
        Console.WriteLine("***========================================================================***");
        foreach (string titleCard in title)
        {
            Console.WriteLine(titleCard);
        }
        Console.WriteLine("***========================================================================***\n");
    }

    static void PrintObject(Array[] arrays, string[] labels, int index)
    {
        for (int i = 0; i < arrays.Length; i++)
        {
            Array array = arrays[i];
            object? value = array.GetValue(index);
            if (value is bool) value = BoolToString((bool) value);

            Console.WriteLine($"\t\t> {labels[i] ?? "NULO"}: {array.GetValue(index) ?? "NULO"}");
        }
    }

    static void PrintObjects(Array[] arrays, string[] labels, int count, string title)
    {
        int totalPages = (count + OBJECTS_PER_PAGE - 1) / OBJECTS_PER_PAGE;
        int page = 1;
        bool enabled = true;

        do
        {
            Console.Clear();
            Title(
                [
                    $"\t\t\t{title}",
                    $"\t\tPagina siguiente : {NEXT_PAGE}",
                    $"\t\tPagina anterior : {PREV_PAGE}",
                    $"\t\tSalir de paginas : {EXIT_PAGES}",
                ]
            );

            if (count == 0)
            {
                Console.WriteLine("\t\tEsta colección está vacia.\n");
            } else
            {
                int minIndex = (page - 1) * OBJECTS_PER_PAGE;
                int maxIndex = Math.Min(page * OBJECTS_PER_PAGE, count);

                for (int i = minIndex; i < maxIndex; i++)
                {   
                    if (i != minIndex)
                    {
                        Console.WriteLine("\t_______________________________________________________________\n");
                    }

                    PrintObject(arrays, labels, i);
                }                
            }

            Console.WriteLine("\t_______________________________________________________________\n");
            Console.WriteLine($"\t\t\t\tPag. {page} de {totalPages}");

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == NEXT_PAGE && page < totalPages)
            {
                page++;
            } else if (key.Key == PREV_PAGE && page > 1)
            {
                page--;
            } else if (key.Key == EXIT_PAGES)
            {
                enabled = false;
            }

        } while (enabled);
    }

    static void PrintSpecialties()
    {
        PrintObjects(
            [
                specialtyName,
                specialtyMedicCount,
                specialtyOfficeAvailability,
                specialtyInCharge
            ],
            [
                "Nombre",
                "Medicos asignados",
                "Consultorios disponibles",
                "Responsable del área"
            ],
            specialtyCount,
            "ESPECIALIDADES EN SISTEMA"
        );
        SpecialtyMenu();
    }

    static void FetchSpecialty()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tCONSULTAR ESPECIALIDAD",
                "\t\tIngrese el nombre de la especialidad a buscar."
            ]
        );
        if (specialtyCount == 0)
        {
            Console.WriteLine("\tAún no hay especialidades registradas.");
        } else
        {
            string nombre = GetInput("Nombre de especialidad a buscar");
            int index = FetchInArray(specialtyName, nombre);

            if (index == -1)
            {
                Console.WriteLine("\tNo se encontro una especialidad con ese nombre.");
            } else
            {
                Console.WriteLine("\tEspecialidad encontrada.\n");
                PrintObject(
                    [
                        specialtyName,
                        specialtyMedicCount,
                        specialtyOfficeAvailability,
                        specialtyInCharge
                    ],
                    [
                        "Nombre",
                        "Medicos asignados",
                        "Consultorios disponibles",
                        "Responsable del área"
                    ],
                    index
                );
            }
        }

        AwaitKey();
        SpecialtyMenu();
    }

    static void RegisterSpecialty()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tREGISTRAR ESPECIALIDAD",
                "\t\tLlene los campos requeridos a continuación."
            ]
        );
        Console.WriteLine();
        if (specialtyCount >= DATA_LENGTH)
        {
            Console.WriteLine("\tNo es posible registrar mas especialidades. Capacidad maxima alcanzada.");
        } else
        {
            string name = GetInput("Nombre de especialidad");
            if (FetchInArray(specialtyName, name) != -1)
            {
                Console.WriteLine("\tNo es posible registrar la especialidad.");
                Console.WriteLine("\t¡Ya existe una especialidad con ese nombre!");
            } else
            {
                int medicCount = GetIntInput("Cantidad de medicos asignados");
                int officeCount = GetIntInput("Cantidad de consultorios disponibles");
                string inCharge = GetInput("Responsable del área");

                specialtyName[specialtyCount] = name;
                specialtyMedicCount[specialtyCount] = medicCount;
                specialtyOfficeAvailability[specialtyCount] = officeCount;
                specialtyInCharge[specialtyCount] = inCharge;
                specialtyCount++;

                Console.WriteLine("\tEspecialidad registrada exitosamente.");
            }
        }

        AwaitKey();
        SpecialtyMenu();
    }

    static void SpecialtyMenu()
    {
        bool valid;
        int selection;

        do
        {
            Console.Clear();
            Title(
                [
                    "\t\t\tESPECIALIDADES",
                    "\t\tMódulo de gestión de especialidades."
                ]
            );
            GetSelection(
                [
                    "Registrar especialidad",
                    "Consultar especialidad",
                    "Mostrar información registrada",
                    "Regresar"
                ],
                out selection,
                out valid
            );
        } while (!valid);

        switch(selection)
        {  
            case 1:
                RegisterSpecialty();
                break;
            case 2:
                FetchSpecialty();
                break;
            case 3:
                PrintSpecialties();
                break;
            default:
                OpenMenu();
                break;
        }
    }

    static void PrintAppointments()
    {
        PrintObjects(
            [
                appointmentNumber,
                appointmentPatient,
                appointmentMedic,
                appointmentDate,
                appointmentTime,
                appointmentType
            ],
            [
                "Número de cita",
                "Paciente",
                "Médico",
                "Fecha",
                "Hora",
                "Tipo de consulta"
            ],
            appointmentCount,
            "CITAS EN SISTEMA"
        );
        AppointmentMenu();
    }

    static void FetchAppointment()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tCONSULTAR CITA",
                "\t\tIngrese el número de cita a buscar."
            ]
        );
        if (appointmentCount == 0)
        {
            Console.WriteLine("\tAún no hay citas registradas.");
        } else
        {
            int number = GetIntInput("Número de cita a buscar");
            int index = FetchInArray(appointmentNumber, number);

            if (index == -1)
            {
                Console.WriteLine("\tNo se encontro una cita con ese número.");
            } else
            {
                Console.WriteLine("\tCita encontrada.\n");
                PrintObject(
                    [
                        appointmentNumber,
                        appointmentPatient,
                        appointmentMedic,
                        appointmentDate,
                        appointmentTime,
                        appointmentType
                    ],
                    [
                        "Número de cita",
                        "Paciente",
                        "Médico",
                        "Fecha",
                        "Hora",
                        "Tipo de consulta"
                    ],
                    index
                );
            }
        }

        AwaitKey();
        AppointmentMenu();
    }

    static void RegisterAppointment()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tREGISTRAR CITA",
                "\t\tLlene los campos requeridos a continuación."
            ]
        );
        Console.WriteLine();
        if (appointmentCount >= DATA_LENGTH)
        {
            Console.WriteLine("\tNo es posible registrar mas citas. Capacidad maxima alcanzada.");
        } else
        {
            int number = GetIntInput("Número de cita");
            if (FetchInArray(appointmentNumber, number) != -1)
            {
                Console.WriteLine("\tNo es posible registrar la cita.");
                Console.WriteLine("\t¡Ya existe una cita con ese número!");
            } else
            {
                string patient = GetInput("Nombre del paciente");
                string medic = GetInput("Nombre del medico");
                string date = GetInput("Fecha de la cita");
                string time = GetInput("Hora de la cita");
                string type = GetOptionInput("Tipo de consulta", APPOINTMENT_TYPE);

                appointmentNumber[appointmentCount] = number;
                appointmentPatient[appointmentCount] = patient;
                appointmentMedic[appointmentCount] = medic;
                appointmentDate[appointmentCount] = date;
                appointmentTime[appointmentCount] = time;
                appointmentType[appointmentCount] = type;
                appointmentCount++;

                Console.WriteLine("\tLa cita se ha registrado exitosamente.");
            }
        }

        AwaitKey();
        AppointmentMenu();
    }

    static void AppointmentMenu()
    {
        bool valid;
        int selection;

        do
        {
            Console.Clear();
            Title(
                [
                    "\t\t\tCITAS",
                    "\t\tMódulo de gestión de citas."
                ]
            );
            GetSelection(
                [
                    "Registrar cita",
                    "Consultar cita",
                    "Mostrar información registrada",
                    "Regresar"
                ],
                out selection,
                out valid
            );
        } while (!valid);

        switch(selection)
        {  
            case 1:
                RegisterAppointment();
                break;
            case 2:
                FetchAppointment();
                break;
            case 3:
                PrintAppointments();
                break;
            default:
                OpenMenu();
                break;
        }
    }

    static void PrintOffices()
    {
        PrintObjects(
            [
                officeNumber,
                officeFloor,
                officeCapacity,
                officeQueue,
                officeSpots,
                officeAvailable,
            ],
            [
                "Número de consultorio",
                "Piso de consultorio",
                "Capacidad de pacientes en espera",
                "Cantidad de pacientes esperando",
                "Espacios disponibles",
                "Disponible"
            ],
            officeCount,
            "CONSULTORIOS EN SISTEMA"
        );
        OfficeMenu();
    }

    static void FetchOffice()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tCONSULTAR CONSULTORIO",
                "\t\tIngrese el número del consultorio a buscar."
            ]
        );
        if (officeCount == 0)
        {
            Console.WriteLine("\tAún no hay consultorios registrados.");
        } else
        {
            int number = GetIntInput("Número a buscar");
            int index = FetchInArray(officeNumber, number);

            if (index == -1)
            {
                Console.WriteLine("\tNo se encontro un consultorio con ese número.");
            } else
            {
                Console.WriteLine("\tConsultorio encontrado.\n");
                PrintObject(
                    [
                        officeNumber,
                        officeFloor,
                        officeCapacity,
                        officeQueue,
                        officeSpots,
                        officeAvailable,
                    ],
                    [
                        "Número de consultorio",
                        "Piso de consultorio",
                        "Capacidad de pacientes en espera",
                        "Cantidad de pacientes esperando",
                        "Espacios disponibles",
                        "Disponible"
                    ],
                    index
                );
            }
        }

        AwaitKey();
        OfficeMenu();
    }

    static void RegisterOffice()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tREGISTRAR CONSULTORIO",
                "\t\tLlene los campos requeridos a continuación."
            ]
        );
        Console.WriteLine();
        if (officeCount >= DATA_LENGTH)
        {
            Console.WriteLine("\tNo es posible registrar mas consultorios. Capacidad maxima alcanzada.");
        } else
        {
            int number = GetIntInput("Número de consultorio");
            if (FetchInArray(officeNumber, number) != -1)
            {
                Console.WriteLine("\tNo es posible registrar el mconsultorioédico.");
                Console.WriteLine("\t¡Ya existe un consultorio con ese número!");
            } else
            {
                int floor = GetIntInput("Piso de consultorio");
                int capacity = GetIntInput("Capacidad de pacientes en espera");
                int queue = GetIntInput("Cantidad de pacientes esperando");
                bool available = GetBoolInput("Consultorio disponible");
                int spots = Math.Max(capacity - queue, 0);

                officeNumber[officeCount] = number;
                officeFloor[officeCount] = floor;
                officeCapacity[officeCount] = capacity;
                officeQueue[officeCount] = queue;
                officeAvailable[officeCount] = available;
                officeSpots[officeCount] = spots;
                officeCount++;

                Console.WriteLine($"\tEspacios disponibles: {spots}\n");
                Console.WriteLine("\tConsultorio registrado exitosamente.");

                if (queue >= capacity)
                {
                    Console.WriteLine("\tADVERTENCIA: Este consultorio esta en su limite de capacidad de pacientes por atender.");
                }
            }
        }

        AwaitKey();
        OfficeMenu();
    }

    static void OfficeMenu()
    {
        bool valid;
        int selection;

        do
        {
            Console.Clear();
            Title(
                [
                    "\t\t\tCONSULTORIOS",
                    "\t\tMódulo de gestión de consultorios."
                ]
            );
            GetSelection(
                [
                    "Registrar consultorio",
                    "Consultar consultorio",
                    "Mostrar información registrada",
                    "Regresar"
                ],
                out selection,
                out valid
            );
        } while (!valid);

        switch(selection)
        {  
            case 1:
                RegisterOffice();
                break;
            case 2:
                FetchOffice();
                break;
            case 3:
                PrintOffices();
                break;
            default:
                OpenMenu();
                break;
        }
    }

    static void PrintMedics()
    {
        PrintObjects(
            [
                medicCode,
                medicName,
                medicSpecialty,
                medicExperience,
                medicSchedule
            ],
            [
                "Codigo de médico",
                "Nombre",
                "Especialidad",
                "Años de experiencia",
                "Horario de atención"
            ],
            medicCount,
            "MÉDICOS EN SISTEMA"
        );
        MedicMenu();
    }

    static void FetchMedic()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tCONSULTAR MÉDICO",
                "\t\tIngrese el código del médico a buscar."
            ]
        );
        if (medicCount == 0)
        {
            Console.WriteLine("\tAún no hay médicos registrados.");
        } else
        {
            int code = GetIntInput("Código a buscar");
            int index = FetchInArray(medicCode, code);

            if (index == -1)
            {
                Console.WriteLine("\tNo se encontro un médico con ese código.");
            } else
            {
                Console.WriteLine("\tMédico encontrado.\n");
                PrintObject(
                    [
                        medicCode,
                        medicName,
                        medicSpecialty,
                        medicExperience,
                        medicSchedule
                    ],
                    [
                        "Codigo de médico",
                        "Nombre",
                        "Especialidad",
                        "Años de experiencia",
                        "Horario de atención"
                    ],
                    index
                );
            }
        }

        AwaitKey();
        MedicMenu();
    }

    static void RegisterMedic()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tREGISTRAR MÉDICO",
                "\t\tLlene los campos requeridos a continuación."
            ]
        );

        Console.WriteLine();
        if (medicCount >= DATA_LENGTH)
        {
            Console.WriteLine("\tNo es posible registrar mas medicos. Capacidad maxima alcanzada.");
        } else if (specialtyCount == 0)
        {
            Console.WriteLine("\tNo es posible registrar medicos. No hay ninguna especialidad registrada.");
        } else
        {
            int code = GetIntInput("Código de médico");
            if (FetchInArray(medicCode, code) != -1)
            {
                Console.WriteLine("\tNo es posible registrar el médico.");
                Console.WriteLine("\t¡Ya existe un médico con ese código!");
            } else
            {
                string name = GetInput("Nombre completo");
                string specialty = GetOptionInput("Especialidad", specialtyName);
                int experience = GetIntInput("Años de experiencia");
                string schedule = GetInput("Horario de atención (ej. 8:00am - 4:00pm)");

                medicCode[medicCount] = code;
                medicName[medicCount] = name;
                medicSpecialty[medicCount] = specialty;
                medicExperience[medicCount] = experience;
                medicSchedule[medicCount] = schedule;
                medicCount++;

                int specialtyIndex = FetchInArray(specialtyName, specialty);
                if (specialtyIndex != -1)
                {
                    specialtyMedicCount[specialtyIndex]++;
                }

                Console.WriteLine("\tMédico registrado exitosamente.");
            }
        }

        AwaitKey();
        MedicMenu();
    }

    static void MedicMenu()
    {
        bool valid;
        int selection;

        do
        {
            Console.Clear();
            Title(
                [
                    "\t\t\tMÉDICOS",
                    "\t\tMódulo de gestión de médicos."
                ]
            );
            GetSelection(
                [
                    "Registrar médico",
                    "Consultar médico",
                    "Mostrar información registrada",
                    "Regresar"
                ],
                out selection,
                out valid
            );
        } while (!valid);

        switch(selection)
        {  
            case 1:
                RegisterMedic();
                break;
            case 2:
                FetchMedic();
                break;
            case 3:
                PrintMedics();
                break;
            default:
                OpenMenu();
                break;
        }
    }

    static void PrintPatients()
    {
        PrintObjects(
            [
                patientFile,
                patientName,
                patientAge,
                patientSex,
                patientBloodType,
                patientPhone
            ],
            [
                "Número de expediente",
                "Nombre completo",
                "Edad",
                "Sexo",
                "Tipo de sangre",
                "Número de teléfono"
            ],
            patientCount,
            "PACIENTES EN SISTEMA"
        );
        PatientMenu();
    }

    static void FetchPatient()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tCONSULTAR PACIENTE",
                "\t\tIngrese el expediente del paciente a buscar."
            ]
        );
        if (patientCount == 0)
        {
            Console.WriteLine("\tAún no hay pacientes registrados.");
        } else
        {
            int file = GetIntInput("Número de expediente a buscar");
            int index = FetchInArray(patientFile, file);

            if (index == -1)
            {
                Console.WriteLine("\tNo se encontro un paciente con ese expediente.");
            } else
            {
                Console.WriteLine("\tPaciente encontrado.\n");
                PrintObject(
                    [
                        patientFile,
                        patientName,
                        patientAge,
                        patientSex,
                        patientBloodType,
                        patientPhone
                    ],
                    [
                        "Número de expediente",
                        "Nombre completo",
                        "Edad",
                        "Sexo",
                        "Tipo de sangre",
                        "Número de teléfono"
                    ],
                    index
                );
            }
        }

        AwaitKey();
        PatientMenu();
    }

    static void RegisterPatient()
    {
        Console.Clear();
        Title(
            [
                "\t\t\tREGISTRAR PACIENTE",
                "\t\tLlene los campos requeridos a continuación."
            ]
        );
        Console.WriteLine();
        if (patientCount >= DATA_LENGTH)
        {
            Console.WriteLine("\tNo es posible registrar mas pacientes. Capacidad maxima alcanzada.");
        } else
        {
            int file = GetIntInput("Número de expediente");
            if (FetchInArray(patientFile, file) != -1)
            {
                Console.WriteLine("\tNo es posible registrar el paciente.");
                Console.WriteLine("\t¡Ya existe un paciente con ese número de expediente!");
            } else
            {
                string name = GetInput("Nombre completo");
                int age = GetIntInput("Edad");
                string sex = GetOptionInput("Sexo", SEX);
                string bloodType = GetOptionInput("Tipo de sangre", BLOOD_TYPE);
                int phone = GetIntInput("Número de teléfono");

                patientFile[patientCount] = file;
                patientName[patientCount] = name;
                patientAge[patientCount] = age;
                patientSex[patientCount] = sex;
                patientBloodType[patientCount] = bloodType;
                patientPhone[patientCount] = phone;
                patientCount++;

                Console.WriteLine("\tPaciente registrado exitosamente.");
            }
        }

        AwaitKey();
        PatientMenu();
    }

    static void PatientMenu()
    {
        bool valid;
        int selection;

        do
        {
            Console.Clear();
            Title(
                [
                    "\t\t\tPACIENTES",
                    "\t\tMódulo de gestión de pacientes."
                ]
            );
            GetSelection(
                [
                    "Registrar paciente",
                    "Consultar paciente",
                    "Mostrar información registrada",
                    "Regresar"
                ],
                out selection,
                out valid
            );
        } while (!valid);

        switch(selection)
        {  
            case 1:
                RegisterPatient();
                break;
            case 2:
                FetchPatient();
                break;
            case 3:
                PrintPatients();
                break;
            default:
                OpenMenu();
                break;
        }
    }

    static void OpenMenu()
    {
        bool valid;
        int selection;

        do
        {
            Console.Clear();
            Title(["\t\t\tCLINICA MEDICA UNICAES", "\t\t\tSistema Administrativo"]);
            GetSelection(
                [
                    "Administración de Pacientes",
                    "Administración de Medicos", 
                    "Administración de Consultorios", 
                    "Administración de Citas", 
                    "Administración de Especialidades", 
                    "Salir" 
                ], 
                out selection, 
                out valid
            );
        } while (!valid);

        switch (selection)
        {
            case 1:
                PatientMenu();
                break;
            case 2:
                MedicMenu();
                break;
            case 3:
                OfficeMenu();
                break;
            case 4:
                AppointmentMenu();
                break;
            case 5:
                SpecialtyMenu();
                break;
            default:
                AwaitKey("Muchas gracias por utilizar el sistema.");
                break;
        }
    }

    static void Main(string[] args)
    {
        OpenMenu();
    }
}