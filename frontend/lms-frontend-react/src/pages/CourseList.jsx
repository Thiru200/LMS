import { useEffect, useState } from "react";
const CourseList = () => {
  const [course, setCourse] = useState([]);
  const [error, setError] = useState("");
  useEffect(() => {
    const fetchCourse = async () => {
      try {
        const res = await fetch("http://localhost:5002/courses/GetCourses"); // Replace with your CourseService API
        if (!res.ok) throw new Error("Failed to fetch courses");
        const data = await res.json();
        console.log(data);
        setCourses(data);
      } catch (err) {
        setError(err.message);
      }
    };
    fetchCourse();
  }, []);
  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <h2 className="text-3xl font-bold text-center text-gray-800 mb-8">
        Available Courses
      </h2>
      {error && <div className="text-red-600 text-center mb-4">{error}</div>}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {courses.map((course) => (
          <div
            key={course.id}
            className="bg-white shadow-lg rounded-lg p-6 hover:shadow-xl transition-shadow"
          >
            <h3 className="text-xl font-semibold mb-2 text-blue-700">
              {course.title}
            </h3>
            <p className="text-gray-600 text-sm mb-3">{course.description}</p>
            <p className="text-sm font-medium text-gray-500">
              Instructor: {course.instructor}
            </p>
            <p className="text-sm text-gray-400 italic">
              Category: {course.category}
            </p>

            {course.videoUrl && (
              <a
                href={course.videoUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="mt-3 inline-block text-blue-600 hover:underline text-sm"
              >
                Watch Video
              </a>
            )}

            {course.pdfFileName && (
              <a
                href={`/pdfs/${course.pdfFileName}`} // Optional: serve static PDF
                target="_blank"
                rel="noopener noreferrer"
                className="ml-4 text-sm text-green-600 hover:underline"
              >
                View PDF
              </a>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};
export default CourseList;
