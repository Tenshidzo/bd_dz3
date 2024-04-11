namespace bd_dz3
{
    class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<int> CourseIds { get; set; }
    }

    class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }

    class Program
    {
        static void Main()
        {
            // Заполним коллекции данными
            List<Student> students = new List<Student>
        {
            new Student { StudentId = 1, Name = "Alice", Age = 20, CourseIds = new List<int> { 1, 2, 3 } },
            new Student { StudentId = 2, Name = "Bob", Age = 22, CourseIds = new List<int> { 2, 3, 4 } },
            new Student { StudentId = 3, Name = "Charlie", Age = 21, CourseIds = new List<int> { 1, 3, 5 } },
            // Добавьте еще студентов при необходимости
        };

            List<Course> courses = new List<Course>
        {
            new Course { CourseId = 1, CourseName = "Math" },
            new Course { CourseId = 2, CourseName = "Physics" },
            new Course { CourseId = 3, CourseName = "Computer Science" },
            new Course { CourseId = 4, CourseName = "Biology" },
            new Course { CourseId = 5, CourseName = "Chemistry" },
            // Добавьте еще курсы при необходимости
        };

            // 1. Вывести имена студентов, которые старше 21 года.
            var studentsOver21 = students.Where(s => s.Age > 21).Select(s => s.Name);
            Console.WriteLine("Students older than 21:");
            foreach (var student in studentsOver21)
            {
                Console.WriteLine(student);
            }
            Console.WriteLine();

            // 2. Найти средний возраст студентов по каждому курсу.
            var averageAgeByCourse = students
                .SelectMany(s => s.CourseIds, (s, c) => new { Student = s, CourseId = c })
                .GroupBy(sc => sc.CourseId, sc => sc.Student.Age)
                .Select(g => new { CourseId = g.Key, AverageAge = g.Average() });

            Console.WriteLine("Average age of students by course:");
            foreach (var item in averageAgeByCourse)
            {
                Console.WriteLine($"CourseId: {item.CourseId}, AverageAge: {item.AverageAge}");
            }
            Console.WriteLine();

            // 3. Вывести названия курсов, на которых учится более двух студентов.
            var coursesWithMoreThan2Students = students
                .SelectMany(s => s.CourseIds, (s, c) => new { Student = s, CourseId = c })
                .GroupBy(sc => sc.CourseId)
                .Where(g => g.Count() > 2)
                .Select(g => g.Key)
                .Join(courses, courseId => courseId, c => c.CourseId, (courseId, c) => c.CourseName);

            Console.WriteLine("Courses with more than 2 students:");
            foreach (var courseName in coursesWithMoreThan2Students)
            {
                Console.WriteLine(courseName);
            }
            Console.WriteLine();

            // 4. Найти студента с наибольшим возрастом.
            var oldestStudent = students.OrderByDescending(s => s.Age).First();
            Console.WriteLine($"Oldest student: {oldestStudent.Name}, Age: {oldestStudent.Age}");
            Console.WriteLine();

            // 5. Вывести имена студентов, у которых нет курсов.
            var studentsWithoutCourses = students
                .Where(s => !s.CourseIds.Any())
                .Select(s => s.Name);
            Console.WriteLine("Students without courses:");
            foreach (var studentName in studentsWithoutCourses)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 6. Найти суммарный возраст студентов на каждом курсе.
            var totalAgeByCourse = students
                .SelectMany(s => s.CourseIds, (s, c) => new { Student = s, CourseId = c })
                .GroupBy(sc => sc.CourseId)
                .Select(g => new { CourseId = g.Key, TotalAge = g.Sum(sc => sc.Student.Age) });

            Console.WriteLine("Total age of students by course:");
            foreach (var item in totalAgeByCourse)
            {
                Console.WriteLine($"CourseId: {item.CourseId}, TotalAge: {item.TotalAge}");
            }
            Console.WriteLine();

            // 7. Вывести имена студентов, у которых есть общие курсы.
            var studentsWithCommonCourses = students
                .SelectMany(s => s.CourseIds, (s, c) => new { Student = s, CourseId = c })
                .GroupBy(sc => sc.CourseId)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Select(sc => sc.Student.Name))
                .Distinct();

            Console.WriteLine("Students with common courses:");
            foreach (var studentName in studentsWithCommonCourses)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 8. Найти средний возраст студентов, у которых есть общие курсы.
            var averageAgeWithCommonCourses = students
                 .Where(s => studentsWithCommonCourses.Contains(s.Name))
                 .Average(s => s.Age);

            Console.WriteLine($"Average age of students with common courses: {averageAgeWithCommonCourses}");
            Console.WriteLine();


            // 9. Вывести имена студентов, у которых средний возраст на курсе больше 20 лет.
            var studentsWithAverageAgeAbove20 = students
                .Select(s => new
                {
                    Student = s,
                    AverageAge = s.CourseIds.Average(cId => students.Where(s2 => s2.CourseIds.Contains(cId)).Average(s2 => s2.Age))
                })
                .Where(s => s.AverageAge > 20)
                .Select(s => s.Student.Name);

            Console.WriteLine("Students with average age on course above 20:");
            foreach (var studentName in studentsWithAverageAgeAbove20)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 10. Найти курс с наибольшим числом студентов.
            var courseWithMostStudents = students
                .SelectMany(s => s.CourseIds)
                .GroupBy(cId => cId)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;

            var courseNameWithMostStudents = courses.First(c => c.CourseId == courseWithMostStudents).CourseName;
            Console.WriteLine($"Course with most students: {courseNameWithMostStudents}");
            Console.WriteLine();

            // 11. Вывести имена студентов, у которых средний возраст на курсе максимален.
            var studentsWithMaxAverageAgeOnCourse = students
                .Select(s => new
                {
                    Student = s,
                    MaxAverageAge = s.CourseIds.Max(cId => students.Where(s2 => s2.CourseIds.Contains(cId)).Average(s2 => s2.Age))
                })
                .Where(s => s.MaxAverageAge == s.Student.CourseIds.Average(cId => students.Where(s2 => s2.CourseIds.Contains(cId)).Average(s2 => s2.Age)))
                .Select(s => s.Student.Name);

            Console.WriteLine("Students with max average age on course:");
            foreach (var studentName in studentsWithMaxAverageAgeOnCourse)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 12. Найти курсы, на которых учится хотя бы один студент старше 25 лет.
            var coursesWithStudentsOver25 = students
                .Where(s => s.Age > 25)
                .SelectMany(s => s.CourseIds)
                .Distinct()
                .Join(courses, cId => cId, c => c.CourseId, (cId, c) => c.CourseName);

            Console.WriteLine("Courses with at least one student older than 25:");
            foreach (var courseName in coursesWithStudentsOver25)
            {
                Console.WriteLine(courseName);
            }
            Console.WriteLine();

            // 13. Вывести имена студентов, у которых возраст на курсе отличается не более чем на 1 год.
            var studentsWithAgeDifferenceOneYear = students
                .Select(s => new
                {
                    Student = s,
                    CourseAges = s.CourseIds.Select(cId => students.Where(s2 => s2.CourseIds.Contains(cId)).Select(s2 => s2.Age).ToList())
                })
                .Where(s => s.CourseAges.All(ca => ca.Max() - ca.Min() <= 1))
                .Select(s => s.Student.Name);

            Console.WriteLine("Students with age difference of one year on course:");
            foreach (var studentName in studentsWithAgeDifferenceOneYear)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 14. Найти курсы, на которых нет студентов.
            var coursesWithoutStudents = courses
                .Where(c => !students.SelectMany(s => s.CourseIds).Contains(c.CourseId))
                .Select(c => c.CourseName);

            Console.WriteLine("Courses without students:");
            foreach (var courseName in coursesWithoutStudents)
            {
                Console.WriteLine(courseName);
            }
            Console.WriteLine();

            // 15. Вывести имена студентов, у которых есть курсы и которые не учатся на курсах "Math" и "Physics".
            var studentsWithoutMathPhysics = students
                .Where(s => s.CourseIds.Any() && !s.CourseIds.Contains(1) && !s.CourseIds.Contains(2))
                .Select(s => s.Name);

            Console.WriteLine("Students with courses not including Math and Physics:");
            foreach (var studentName in studentsWithoutMathPhysics)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 16. Найти студентов, у которых есть курсы и которые учатся хотя бы на одном курсе в каждой категории (Math, Physics, Computer Science).
            var studentsInEveryCategory = students
                .Where(s => s.CourseIds.Contains(1) && s.CourseIds.Contains(2) && s.CourseIds.Contains(3))
                .Select(s => s.Name);

            Console.WriteLine("Students in every category (Math, Physics, Computer Science):");
            foreach (var studentName in studentsInEveryCategory)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 17. Вывести имена студентов, у которых есть курсы, и у каждого курса есть хотя бы один другой курс с тем же количеством студентов.
            var studentsWithSameNumberOfStudentsOnEveryCourse = students
                .Select(s => new
                {
                    Student = s,
                    CourseStudentCounts = s.CourseIds.Select(cId => students.Count(s2 => s2.CourseIds.Contains(cId))).Distinct()
                })
                .Where(s => s.CourseStudentCounts.All(csc => s.CourseStudentCounts.Count() == 1 || s.CourseStudentCounts.Count(c => c == csc) > 1))
                .Select(s => s.Student.Name);

            Console.WriteLine("Students with same number of students on every course:");
            foreach (var studentName in studentsWithSameNumberOfStudentsOnEveryCourse)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 18. Найти студентов, у которых есть хотя бы один курс, и у всех их курсов есть другие курсы с тем же количеством студентов.
            var studentsWithCoursesHavingSameNumberOfStudents = students
                .Select(s => new
                {
                    Student = s,
                    CourseIdsWithSameStudentCount = s.CourseIds
                        .SelectMany(cId => students
                            .Where(s2 => s2.CourseIds.Contains(cId) && s2.StudentId != s.StudentId)
                            .SelectMany(s2 => s2.CourseIds)
                            .GroupBy(cId2 => cId2)
                            .Where(g => g.Count() == students.Count(s3 => s3.CourseIds.Contains(g.Key)))
                            .Select(g => g.Key)
                        )
                })
                .Where(s => s.CourseIdsWithSameStudentCount.Count() == s.Student.CourseIds.Count)
                .Select(s => s.Student.Name);

            Console.WriteLine("Students with courses having same number of students:");
            foreach (var studentName in studentsWithCoursesHavingSameNumberOfStudents)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 19. Вывести имена студентов, у которых есть хотя бы один курс, и у всех их курсов средний возраст студентов больше 18 лет.
            var studentsWithCoursesAverageAgeAbove18 = students
                .Select(s => new
                {
                    Student = s,
                    CourseAges = s.CourseIds.Select(cId => students.Where(s2 => s2.CourseIds.Contains(cId)).Select(s2 => s2.Age).ToList())
                })
                .Where(s => s.CourseAges.All(ca => ca.Average() > 18))
                .Select(s => s.Student.Name);

            Console.WriteLine("Students with courses average age above 18:");
            foreach (var studentName in studentsWithCoursesAverageAgeAbove18)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();

            // 20. Найти студентов, у которых средний возраст на всех курсах выше среднего возраста студентов в целом.
            var averageAgeAllStudents = students.Average(s => s.Age);
            var studentsWithAverageAgeAboveOverallAverage = students
                .Select(s => new
                {
                    Student = s,
                    AverageAgeOnAllCourses = s.CourseIds.Average(cId => students.Where(s2 => s2.CourseIds.Contains(cId)).Average(s2 => s2.Age))
                })
                .Where(s => s.AverageAgeOnAllCourses > averageAgeAllStudents)
                .Select(s => s.Student.Name);

            Console.WriteLine("Students with average age on all courses above overall average:");
            foreach (var studentName in studentsWithAverageAgeAboveOverallAverage)
            {
                Console.WriteLine(studentName);
            }
            Console.WriteLine();
        }
    }
}
