import { useState, useEffect } from "react";
const initialState = {
  title: "",
  description: "",
  instructor: "",
  category: "",
  contentHtml: "",
  videoUrl: "",
  pdfFileName: "",
};
const CourseForm = ({ onSave, editCourse }) => {
  const [formData, setFormData] = useState(initialState);
  useEffect(() => {
    if (editCourse) setFormData(editCourse);
  }, [editCourse]);
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };
  const handleSubmit = (e) => {
    e.preventDefault();
    onSave(formData);
    setFormData(initialState);
  };
  return (
    <form
      className="space-y-4 bg-white p-6 rounded shadow"
      onSubmit={handleSubmit}
    >
      <input
        name="title"
        placeholder="Title"
        value={formData.title}
        onChange={handleChange}
        className="w-full border px-3 py-2"
      />
      <textarea
        name="description"
        placeholder="Description"
        value={formData.description}
        onChange={handleChange}
        className="w-full border px-3 py-2"
      />
      <input
        name="instructor"
        placeholder="Instructor"
        value={formData.instructor}
        onChange={handleChange}
        className="w-full border px-3 py-2"
      />
      <input
        name="category"
        placeholder="Category"
        value={formData.category}
        onChange={handleChange}
        className="w-full border px-3 py-2"
      />
      <input
        name="contentHtml"
        placeholder="HTML Content"
        value={formData.contentHtml}
        onChange={handleChange}
        className="w-full border px-3 py-2"
      />
      <input
        name="videoUrl"
        placeholder="YouTube URL"
        value={formData.videoUrl}
        onChange={handleChange}
        className="w-full border px-3 py-2"
      />
      <input
        name="pdfFileName"
        placeholder="PDF File Name"
        value={formData.pdfFileName}
        onChange={handleChange}
        className="w-full border px-3 py-2"
      />

      <button
        type="submit"
        className="bg-blue-600 text-white px-4 py-2 rounded"
      >
        {editCourse ? "Update" : "Add"} Course
      </button>
    </form>
  );
};
export default CourseForm;
