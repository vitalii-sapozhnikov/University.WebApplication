## Annotation
The work considers the processes associated with the scientific
and methodological activities of higher education institutions, examines the
features of the university's subject area in the environment of the I. I. Mechnikov
ONU, identifies the functionality that needs to be implemented, and collects user
requirements.

A database scheme was designed. A three-tier system architecture and MVC
design pattern were chosen. A technology stack was selected for development:
Postgres DBMS, ASP .NET Core platform for the server side and HTML, CSS,
JavaScript, Bootstrap library for the client interface. User interaction with the
system is implemented in compliance with security rules at all levels of the
application.

The information system is implemented with the functionality of evaluating
the effectiveness of lecturers, monitoring the relevance of the lecturer’s
publications to his or her discipline, planning publications by the department,
monitoring the completeness of the discipline with materials, checking whether the
lecturer has fulfilled the licensing conditions related to scientific and
methodological publications, and selecting materials related to a particular
discipline.

## Project aim
The aim of this project is to reduce the number of errors and the time spent on auditing and controlling the activities of the department by creating an information system to support scientific and methodological activities in a higher education institution.  The system being developed allows

- **the head of the department** to analyse the effectiveness of a particular teacher, the activities of the department, plan publications for the calendar year, and check that the teacher meets certain licensing requirements;
- **the guarantor of the educational programme** to check the relevance of the teacher's publications to the discipline he or she teaches, to review the completeness of the discipline with methodological materials;
- **the academic department** to monitor the implementation of publication plans of all departments;
- for **the lecturer** to review the planned publications, add new scientific or methodological publications;
- for **the student** to use the online library of scientific and methodological publications with the possibility of selecting materials in a particular discipline.


## Programme architecture
The software architecture of the application is based on two key
components: ASP.NET Core WebAPI and ASP.NET Core MVC. These components are
are tightly integrated to provide functionality and user interface
interface.
![image](https://github.com/user-attachments/assets/f8df9b3a-7c1d-4c34-b228-1057531f47c8)



## Database integration
All tables in the database of the IS for supporting scientific and methodological 
activities of higher education institutions are in the normal Boyce-Codd form. 
An ER diagram showing the structure of the model of this information system is shown in Figure.
![image](https://github.com/user-attachments/assets/7144b994-3c32-425d-96e1-0a058a17e76c)

## The following UML diagram describes the interaction of a WebAPI with the database.
<img src="https://github.com/user-attachments/assets/fcc1eb52-5099-4ef3-8728-7609a065f240" width=500>


## Webpages hierarchy
![image](https://github.com/user-attachments/assets/545e9e09-ce72-4b54-980b-9d96676fff27)


# Screenshots of working system
## Login page
![image](https://github.com/user-attachments/assets/c3a56c69-36f1-4342-8b31-91a888658c21)
## Publications page
![image](https://github.com/user-attachments/assets/f81e533a-9838-4009-bb4c-d3419cdf9c17)
## Publication details
![Sapozhnikov-Theses Image 55](https://github.com/user-attachments/assets/883ffbd0-1d2a-4e78-8ef5-83addb948efa)
## Plan of department publications for lecturers
![image](https://github.com/user-attachments/assets/a9cff39f-68e6-442e-b1f1-58ce1662ef8f)
## Lecturer's personal account
![image](https://github.com/user-attachments/assets/020f8923-5d6a-4c98-8ea8-a41cb83f8b6b)
## Auto-generated publications plan document
![image](https://github.com/user-attachments/assets/2d49afde-55a8-4f60-9279-ba202397a23c)
## Page "Effectiveness of the department" with filters
![image](https://github.com/user-attachments/assets/5efabfc9-0f31-4f60-a66d-0a1a34318610)
## Correlation between lecturers publications and his subjects
![Sapozhnikov-Theses Image 64](https://github.com/user-attachments/assets/99d6c92c-228c-4d73-b283-2d527edb11ae)




