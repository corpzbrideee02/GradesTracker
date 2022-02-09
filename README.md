# GradesTracker

## This project has two parts:
1. It is composed of a JSON Schema describing some rules for grades data relating to all courses taken by a student. Also, there is a JSON data file that conforms to the schema.
2. A C# console program reads the JSON file into memory and then facilitates the following operations:
* Add, edit or delete a course
* Add evaluations to a course as well as edit and delete evaluations
* Display a summary listing of the grades information for all courses in the data set
* Display a detailed listing of the grades information for all evaluations in a selected course
* Validate any new data generated against the schema created in part 1. 
* At the end of each session, the program should save the data to the JSON data file from step 1.
