using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; //JSonConvert class

namespace CourseNamespace
{
    class Course
    {
        public string Course_Code { get; set; }

        public List<Evaluations> Evaluation_List = new List<Evaluations>();

        public void AddEvaluations(Evaluations item)
        {
            Evaluation_List.Add(item);
        }
        public void DeleteEvaluation(Evaluations item)
        {
            Evaluation_List.Remove(item);
        }
        public void UpdateEvaluation(Evaluations item, double earned_marks)
        {
            //Evaluation_List.Remove(item);
            //Evaluations found= Evaluation_List.FirstOrDefault(c => c.Earned_Marks == earned_marks);
            item.Earned_Marks = earned_marks;
        }
    }

    class Evaluations
    {

        public string Description { get; set; }
        public double Course_Weight { get; set; }

        public int Out_Of { get; set; }
       // [JsonIgnore]
        public double? Earned_Marks { get; set; }

    }

    class CourseCollection
    {

        
        public List<Course> Courses_List = new List<Course>();
        public void AddCourse(Course course)
        {
            Courses_List.Add(course);
        }
        public void DeleteCourse(Course course)
        {
            Courses_List.Remove(course);
        }
    }
}


