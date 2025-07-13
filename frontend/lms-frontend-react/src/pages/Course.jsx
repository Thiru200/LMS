import { useEffect, useState } from "react";

const Course = () => {
  const [courses, setCourses] = useState([]); // ✅ Must be declared
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchCourses = async () => {
      //alert("Hello");
      try {
        const res = await fetch("http://localhost:5002/courses/GetCourses"); // Adjust if your route is different
        if (!res.ok) throw new Error("Failed to fetch courses");
        const data = await res.json();
        console.log(data);
        setCourses(data);
      } catch (err) {
        setError(err.message);
      }
    };

    fetchCourses();
  }, []);

  return (
    <div className="min-h-screen bg-gray-100 py-10 px-4">
      <h2 className="text-3xl font-bold text-center text-gray-800 mb-8">
        Available Courses
      </h2>
      {error && <div className="text-red-600 text-center mb-4">{error}</div>}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {courses.map((course) => {
          const videoIdMatch = course.videoUrl?.match(
            /(?:v=|\/embed\/|youtu\.be\/)([^?&/\s]+)/
          );
          const videoId = videoIdMatch?.[1];

          const thumbnailUrl = videoId
            ? `https://img.youtube.com/vi/SqcY0GlETPk/0.jpg`
            : "https://via.placeholder.com/320x180?text=No+Thumbnail";
          console.log(thumbnailUrl);
          return (
            <div
              key={course.id}
              className="bg-white shadow-lg rounded-lg overflow-hidden hover:shadow-xl transition-shadow"
            >
              {thumbnailUrl && (
                <img
                  src={thumbnailUrl}
                  alt="Video Thumbnail"
                  className="w-full h-48 object-cover"
                />
              )}

              <div className="p-6">
                <h3 className="text-xl font-semibold mb-2 text-blue-700">
                  {course.title}
                </h3>
                <p className="text-gray-600 text-sm mb-3">
                  {course.description}
                </p>
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
                    className="block text-blue-600 mt-2 text-sm hover:underline"
                  >
                    🎬 Watch Video
                  </a>
                )}

                {course.pdfFileName && (
                  <a
                    href={`/pdfs/${course.pdfFileName}`}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="block text-green-600 mt-1 text-sm hover:underline"
                  >
                    📄 View PDF
                  </a>
                )}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default Course;
