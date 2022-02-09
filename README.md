# GradesTracker

This project has two parts:
  1. It is composed of a JSON Schema describing some rules for grades data relating to all courses taken by a student. Also, there is a JSON data file that conforms to the schema.
  2. A C# console program reads the JSON file into memory and then facilitates the following operations:
      a. Add, edit or delete a course
      b. Add evaluations to a course as well as edit and delete evaluations
      c. Display a summary listing of the grades information for all courses in the data set
      d. Display a detailed listing of the grades information for all evaluations in a selected course
      e. Validate any new data generated against the schema created in part 1. At the end of each session, the program should save the data to the JSON data file from step 1.
