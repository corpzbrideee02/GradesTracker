
using System;
using CourseNamespace;
using System.IO;            // FileStream class
using Newtonsoft.Json;      // JsonConvert class
using Newtonsoft.Json.Schema;       // JSchema class
using Newtonsoft.Json.Linq;         // JObject class

namespace ConsoleApp1
{
    class Program
    {
        private static readonly string JSON_FILE = "course_data.json";
        private const string SCHEMA_FILE = "course-schema.json"; 

        public static void PrintingLine()
        {
            for (int i = 0; i < 22; i++) Console.Write("----");
        }

        public static void PrintSectionTitle(string section, string course_code, string evaluation)
        {
            Console.WriteLine("\t\t\t\t~ GRADES TRACKING SYSTEM ~\t\t\t\t");

            PrintingLine();
            //if section is the main page
            if (section=="main")
                Console.WriteLine("\n\t\t\t\t\tGrades Summary\t\t\t\t\t");

            //else if print the course code and EVALUATION LIST
            else if(section=="evaluation_list")
                Console.Write($"\n\t\t\t\t {course_code} Evaluations\t\t\t\n");

            //else print the course code and EVALUATION
            else if (section == "evaluation")
                Console.Write($"\n\t\t\t\t {course_code} {evaluation} \t\t\t\n");

            PrintingLine();
            Console.WriteLine();
        }

        public static void SelectMainCommand()
        {
            PrintingLine();
            Console.WriteLine("\nPress # from the above list to view/edit/delete a specific course.");
            Console.WriteLine("Press A to add a new course.");
            Console.WriteLine("Press X to quit.");
            PrintingLine();
            Console.WriteLine();
        }

        public static void SelectEvaluationListCommand()
        {
            PrintingLine();
            Console.WriteLine("\nPress D to delete this course.");
            Console.WriteLine("Press A to add an evaluation.");
            Console.WriteLine("Press # from the above list to edit/delete a specific evaluation.");
            Console.WriteLine("Press X to return to the main menu.");
            PrintingLine();
            Console.WriteLine();
        }

        public static void SelectEvaluationCommand()
        {
            PrintingLine();
            Console.WriteLine("\nPress D to delete this evaluation.");
            Console.WriteLine("Press E to edit this evaluation.");
            Console.WriteLine("Press X to return to the previous menu.");
            PrintingLine();
            Console.WriteLine();
        }

        public static double ComputePercentEvaluation(Evaluations ev)
        {
            return (ev.Earned_Marks == null) ? 0.0 : (double)(100 * (ev.Earned_Marks / ev.Out_Of));
        }

        public static double ComputeCourseMarks(Evaluations ev)
        {
            return (ComputePercentEvaluation(ev) * ev.Course_Weight) / 100;
        }

        public static void ShowCourseList(CourseCollection cc)
        {
            if(cc.Courses_List.Count==0)
                Console.WriteLine("There are currently no saved courses.");

            else
            {
                String data = String.Format("{0,-5} {1,-25} {2,15} {3, 15}{4, 15} \n", "#.", "Course", "Marks Earned", "Out Of", "Percent");
                double courseMarks_total = 0;
                double weight_total = 0;
                foreach (Course item in cc.Courses_List)
                {
                    double percent_total;
                    if (item.Evaluation_List.Count > 0)
                    {

                        foreach (Evaluations ev in item.Evaluation_List)
                        {
                            courseMarks_total += ComputeCourseMarks(ev);
                            weight_total += (double)(ev.Course_Weight);
                        }
                        percent_total = 100 * (courseMarks_total / weight_total);
                    }
                    else
                    {
                        courseMarks_total = 0;
                        weight_total = 0;
                        percent_total = 0;
                    }
                    data += String.Format("{0,-5} {1,-25} {2,15} {3, 15}{4, 15}\n",cc.Courses_List.IndexOf(item) + 1, item.Course_Code, DecimalConversion(courseMarks_total), DecimalConversion(weight_total), DecimalConversion(percent_total));
                    courseMarks_total = 0;
                    weight_total = 0;
                    percent_total = 0;

                }
            Console.WriteLine($"\n{data}");
            }
        }
        public static string DecimalConversion(double? value)
        {
            return string.Format("{0:0.0}", Math.Round((decimal)(value * 10)) / 10);
        }
        public static void ShowEvaluationList(CourseCollection cc, string cd)
        {
            foreach (Course item in cc.Courses_List)
                {
                    if (item.Course_Code.Equals(cd))
                    {
                        if (item.Evaluation_List.Count == 0)
                            Console.WriteLine($"There are currently no evaluations for {cd}.");
                        else
                        {
                            String data = String.Format("{0,-5} {1,-20} {2,13} {3, 10}{4, 10}{5, 14}{6, 13} \n", "#.", "Evaluation", "Marks Earned", "Out Of", "Percent", "Course Marks", "Weight/100");            
                            foreach (Evaluations ev in item.Evaluation_List)
                            {
                                if(ev.Earned_Marks==null)
                                {
                                     data += String.Format("{0,-5} {1,-20} {2,13} {3, 10}{4, 10}{5, 14}{6, 13}\n", item.Evaluation_List.IndexOf(ev) + 1, ev.Description," ",
                                       DecimalConversion(ev.Out_Of),
                                       DecimalConversion(ComputePercentEvaluation(ev)),
                                       DecimalConversion(ComputeCourseMarks(ev)),
                                       DecimalConversion(ev.Course_Weight));
                            }
                                else
                                {
                                    data += String.Format("{0,-5} {1,-20} {2,13} {3, 10}{4, 10}{5, 14}{6, 13}\n", item.Evaluation_List.IndexOf(ev) + 1, ev.Description,
                                           DecimalConversion(ev.Earned_Marks),
                                           DecimalConversion(ev.Out_Of),
                                           DecimalConversion(ComputePercentEvaluation(ev)),
                                           DecimalConversion(ComputeCourseMarks(ev)),
                                           DecimalConversion(ev.Course_Weight));

                                 }
                             }
                            Console.WriteLine($"\n{data}");
                        }
                    }
                }//end For each
        }

        public static void ShowEvaluation(CourseCollection cc, string cd, int index)
        {
            foreach (Course item in cc.Courses_List)
            {
                if (item.Course_Code.Equals(cd))
                {
                 String data = String.Format("{0,15} {1, 10}{2, 10}{3, 15}{4, 10} \n", "Marks Earned", "Out Of", "Percent", "Course Marks", "Weight/100");
                 foreach (Evaluations ev in item.Evaluation_List)
                   {
                     if (index == (item.Evaluation_List.IndexOf(ev) + 1))
                        {
                            if(ev.Earned_Marks==0)
                            {
                                data += String.Format("{0,15} {1, 10}{2, 10}{3, 15}{4, 10}\n", 
                                    " ", ev.Out_Of, DecimalConversion(ComputePercentEvaluation(ev)), DecimalConversion(ComputeCourseMarks(ev)), DecimalConversion(ev.Course_Weight));
                            }
                            else
                            {
                                data += String.Format("{0,15} {1, 10}{2, 10}{3, 15}{4, 10}\n",
                                    ev.Earned_Marks, ev.Out_Of, DecimalConversion(ComputePercentEvaluation(ev)), DecimalConversion(ComputeCourseMarks(ev)), DecimalConversion(ev.Course_Weight));
                            }
                        }
                   }
                  Console.WriteLine($"\n{data}");
               }

            }//end For each
        }


        public  static void Main(string[] args)
        {
            CourseCollection cc = null;

            try
            {
                cc = ReadJsonFileToLib();
            }
            catch (IOException)
            {
                Console.WriteLine($"{JSON_FILE} not found.");
            }
            //if there's no course list, create a new one
            if (cc == null)
            {
                Console.WriteLine("Creating new data set...Press any key to continue...");
                Console.ReadLine();
                Console.Clear();
                cc = new CourseCollection();
            }

            //if the schema file is found, go to the main menu and starts the main program
            string json_schema;
            if (ReadFile(SCHEMA_FILE, out json_schema))
            {
                PrintSectionTitle("main", null, null);
                ShowCourseList(cc);
                SelectMainCommand();

                bool main_Done;
                char select_cmd;
                int select_num_cmd;
                do
                {
                    Console.Write("Enter a main command: ");
                    select_cmd = Convert.ToChar(Console.Read());

                    //selecting ADD A NEW COURSE
                    if (select_cmd.Equals('A'))
                    {
                        Console.ReadLine();
                        bool valid;
                        do
                        {
                            Course crs = new Course();
                            Console.Write("Enter a course code: ");
                            crs.Course_Code = Console.ReadLine();
                            cc.AddCourse(crs);
                            // Validate the item object against the schema using the ValidateItem() method, and IF the object is invalid, repeat the user-input code above to repopulate the object until it's valid
                            valid = ValidateItem(cc, json_schema);

                            if (!valid)
                            {
                                Console.WriteLine($"\nERROR:\tInvalid course code. String '{crs.Course_Code}' does not match regex pattern.");
                                cc.DeleteCourse(crs);
                            }
                        } while (!valid);

                        //show Course List
                        Console.Clear();
                        PrintSectionTitle("main", null, null);
                        ShowCourseList(cc);
                        SelectMainCommand();
                        main_Done = false;
                    }
                    //selecting QUIT
                    else if (select_cmd.Equals('X'))
                    {
                        Console.Write("Exit main menu... ");
                        main_Done = true;
                    }
                    //select num of course
                    else
                    {
                        Console.ReadLine();
                        select_num_cmd = (int)Char.GetNumericValue((select_cmd));

                        string selected_course = "", sel_eval_list_cmd;
                        bool eval_list_Done;
                        foreach (Course item in cc.Courses_List)
                        {
                            if (select_num_cmd == (cc.Courses_List.IndexOf(item) + 1))
                                selected_course = item.Course_Code;
                        }
                        Console.Clear();
                        PrintSectionTitle("evaluation_list", selected_course,null);
                        ShowEvaluationList(cc, selected_course);
                        SelectEvaluationListCommand();

                        //loop until selecting X to return to the main menu
                        do
                        {
                            Console.Write("Enter an evaluation list command: ");
                            sel_eval_list_cmd = Console.ReadLine();

                            //selecting ADD A NEW EVALUATIONS
                            if (sel_eval_list_cmd.Equals("A"))
                            {
                                foreach (Course item in cc.Courses_List)
                                {
                                    if (select_num_cmd == (cc.Courses_List.IndexOf(item) + 1))
                                    {
                                        bool valid_evaluation;
                                        do
                                        {
                                            Evaluations eval = new Evaluations();

                                            Console.Write("Enter a description: ");
                                            eval.Description = Console.ReadLine();

                                            ValidateNumber(eval, "'out of' mark: ", "'Out of Mark' must be an integer", "int");
                                            ValidateNumber(eval, "% weight: ", "'the % weight' must be a number", "double");
                                            ValidateEarnedMarks(eval);
                                            item.Evaluation_List.Add(eval);

                                            valid_evaluation = ValidateItem(cc, json_schema);

                                            if (!valid_evaluation)
                                            {
                                                Console.WriteLine("\nERROR:\tInvalid Evaluation. Please try again.\n");
                                                item.Evaluation_List.Remove(eval);
                                            }

                                        } while (!valid_evaluation);

                                       
                                        Console.Clear();
                                        PrintSectionTitle("evaluation_list", selected_course,null);
                                        ShowEvaluationList(cc, selected_course);
                                        SelectEvaluationListCommand();

                                    }//end of "if selected"

                                }//end of first foreach

                                eval_list_Done = false;

                            }//end  IF(Select= 'A')

                            //selecting DELETING COURSE
                            else if (sel_eval_list_cmd.Equals("D"))
                            {
                                Course temp_item = null;
                                char ans = ' ';
                                foreach (Course item in cc.Courses_List)
                                {
                                    if (select_num_cmd == (cc.Courses_List.IndexOf(item) + 1))
                                    {
                                        temp_item = item;
                                        Console.Write($"\nDelete {item.Course_Code} ? (Y/N): ");
                                        ans = Console.ReadKey().KeyChar;
                                        
                                    }
                                }

                                //course will only be deleted if user selected 'Y'
                                if (ans.Equals('Y'))
                                    cc.DeleteCourse(temp_item);

                                //show COurse List
                                Console.Clear();
                                PrintSectionTitle("main", null,null);
                                ShowCourseList(cc);
                                SelectMainCommand();
                                Console.ReadLine();
                                eval_list_Done = true;
                            }//endIF

                             //selecting RETURN to main menu
                             else if (sel_eval_list_cmd.Equals("X"))
                            {
                                Console.Write("Returning to main menu... ");
                                Console.Clear();
                                PrintSectionTitle("main", null,null);
                                ShowCourseList(cc);
                                SelectMainCommand();
                                eval_list_Done = true;
                            }
                            //selecting evaluation number from above list
                            else
                            {
                                bool evaluation_Done = true;
                                int select_eval_num;
                                select_eval_num = int.Parse(sel_eval_list_cmd);

                                string selected_evaluation_number = "", selected_out_of = "";
                                char selected_evaluation_cmd;

                                Evaluations temp_item = null;
                                Course temp_course = null;
                                foreach (Course item in cc.Courses_List)
                                {
                                    if (select_num_cmd == (cc.Courses_List.IndexOf(item) + 1))
                                    {
                                        temp_course = item;
                                        foreach (Evaluations ev in item.Evaluation_List)
                                        {
                                            if (select_eval_num == (item.Evaluation_List.IndexOf(ev) + 1))
                                            {
                                                selected_evaluation_number = ev.Description;
                                                selected_out_of = ev.Out_Of.ToString();
                                                temp_item = ev;
                                            }
                                        }
                                    }
                                       

                                }

                                Console.Clear();
                                PrintSectionTitle("evaluation", selected_course, selected_evaluation_number);
                                ShowEvaluation(cc, selected_course, select_eval_num);
                                SelectEvaluationCommand();

                                //single evaluation command
                                do
                                {
                                    Console.Write("Enter a single evaluation command: ");
                                    selected_evaluation_cmd = Console.ReadKey().KeyChar;

                                    //selecting EDIT EVALUATION
                                    if (selected_evaluation_cmd.Equals('E'))
                                    {
                                        bool valid_earned_marks;
                                        do
                                        {
                                            Console.Write($"\nEnter marks earned out of {selected_out_of}, press ENTER to leave unassigned: ");
                                            string data_mark_entered = Console.ReadLine();
                                            double marks_entered;
                                            valid_earned_marks = double.TryParse(data_mark_entered, out marks_entered) || data_mark_entered == "";

                                            if (!valid_earned_marks)
                                                Console.WriteLine("\nERROR:\tInvalid 'marks earned'. Please try again.\n");
                                            else

                                                temp_course.UpdateEvaluation(temp_item, marks_entered);

                                        } while (!valid_earned_marks);


                                        //show Evaluation List
                                        Console.Clear();
                                        PrintSectionTitle("evaluation_list", selected_course, null);
                                        ShowEvaluationList(cc, selected_course);
                                        SelectEvaluationListCommand();
                                        evaluation_Done = true;

                                    }//end  IF(Select= 'E')

                                    //selecting DELETING EVALUATION
                                    else if (selected_evaluation_cmd.Equals('D'))
                                    {
                                        char ans = ' ';
                                        Console.Write($"\nDelete {selected_evaluation_number} ? (Y/N): ");
                                        ans = Console.ReadKey().KeyChar;
                                        
                                        if (ans.Equals('Y'))
                                            temp_course.DeleteEvaluation(temp_item);

                                        //show Evaluation List
                                        Console.Clear();
                                        PrintSectionTitle("evaluation_list", selected_course, null);
                                        ShowEvaluationList(cc, selected_course);
                                        SelectEvaluationListCommand();
                                        evaluation_Done = true;
                                    }//endIF

                                    //selecting RETURN to previous menu
                                    else if (selected_evaluation_cmd.Equals('X'))
                                    {
                                        Console.Write("Returning to previous menu... ");

                                        //show Evaluation List
                                        Console.Clear();
                                        PrintSectionTitle("evaluation_list", selected_course, null);
                                        ShowEvaluationList(cc, selected_course);
                                        SelectEvaluationListCommand();
                                        evaluation_Done = true;

                                    }

                                } while (!evaluation_Done);

                                eval_list_Done = false;

                            }//END OF slecting number in the evaluation list

                        } while (!eval_list_Done);

                        main_Done = false; 
                    }

                }
                while (!main_Done);

                //PRINT TO JSON FILE
                try { WriteToJsonFile(cc); }
                catch (IOException) { Console.WriteLine("ERROR: Can't write to the JSON file."); }
            
            }//end if Readfile()

            else //else, unable to read file
                Console.WriteLine("\nERROR:\tUnable to read the schema file.");  // Read operation for schema failed

        }//end of Main method

        // Uses the JsonConvert class to parse the JSON file and create a CourseCollection object
        private static CourseCollection ReadJsonFileToLib()
        {
            string json = File.ReadAllText(JSON_FILE);
            return JsonConvert.DeserializeObject<CourseCollection>(json);
        } 

        // Uses the JsonConvert class to serialize a Library object and write it in JSON format to a file
        private static void WriteToJsonFile(CourseCollection cc)
        {
            string json = JsonConvert.SerializeObject(cc);
            File.WriteAllText(JSON_FILE, json);
            Console.WriteLine($"\n\nCourse list has been written to {JSON_FILE}.\n");
        } 

        // A helper method with a type parameter that allows data in a string to  be converted to any nullable value type
        private static T? GetValueOrNull<T>(string value) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
                return (T)Convert.ChangeType(value, typeof(T));
        } 

        // Attempts to read the json file specified by 'path' into the string 'json', Returns 'true' if successful or 'false' if it fails
        private static bool ReadFile(string path, out string json)
        {
            try
            {
                json = File.ReadAllText(path); // Read JSON file data 
                return true;
            }
            catch
            {
                json = null;
                return false;
            }
        } 

        // Validates an item object against a schema (incomplete)
        private static bool ValidateItem(CourseCollection course, string json_schema)
        {
            // Convert item object to a JSON string 
            string json_data = JsonConvert.SerializeObject(course);

            // Validate the data string against the schema contained in the json_schema parameter. Also, modify or replace the following , return statement to return 'true' if item is valid, or 'false'  if invalid.
            JSchema schema = JSchema.Parse(json_schema);
            JObject itemObj = JObject.Parse(json_data);
            return itemObj.IsValid(schema);
        } 


        //Number Validation
        private static void ValidateNumber(Evaluations eval, string promptMessage, string ErrorMessage, string type)
        {
            bool valid_number = false;
            do
            {
                Console.Write($"Enter the {promptMessage}");
                string data = Console.ReadLine();

                if(type.Equals("int"))
                {
                    valid_number = int.TryParse(data, out int temp);

                    if (valid_number)
                        eval.Out_Of = temp;
                    else
                        Console.WriteLine($"\tERROR: {ErrorMessage}");
                }
                else if (type.Equals("double"))
                {
                    valid_number = double.TryParse(data, out double temp);

                    if (valid_number)
                        eval.Course_Weight = temp;
                    else
                        Console.WriteLine($"\tERROR: {ErrorMessage}");
                }
               
            } while (!valid_number);
        }

        //validate earned marks
        private static void ValidateEarnedMarks(Evaluations eval)
        {
            bool valid_earned_marks;
            do
            {
                Console.Write("Enter marks earned or press ENTER to skip: ");
                string data_mark_entered = Console.ReadLine();
                double marks_entered;
                valid_earned_marks = double.TryParse(data_mark_entered, out marks_entered) || data_mark_entered == "";

                if (!valid_earned_marks)
                    Console.WriteLine("\nERROR:\tInvalid 'marks earned'. Please try again.\n");
                else
                    eval.Earned_Marks = GetValueOrNull<double>(data_mark_entered);

            } while (!valid_earned_marks);
        }
    }
}
