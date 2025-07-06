import { useState } from "react";
import { useNavigate } from "react-router-dom";
const Login = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    email: "",
    password: "",
  });
  const [error, setError] = useState("");
  const handleChange = (e) => {
    setForm((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };
  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const res = await fetch("http://localhost:5001/api/users/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(form),
      });
      if (!res.ok) {
        const { message } = await res.json();
        throw new Error(message || "Login failed");
      }
      const data = await res.json();
      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(data.user));
      navigate("/courses");
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div
      className="min-h-screen bg-cover bg-center relative"
      style={{
        backgroundImage:
          "url('https://images.unsplash.com/photo-1584697964154-943ff9a2ea2d')",
      }}
    >
      <div className="absolute inset-0 bg-black bg-opacity-60 z-0" />
      {/* Centered form card */}
      <div className="relative z-10 flex items-center justify-center min-h-screen px-4">
        <form
          onSubmit={handleSubmit}
          className="bg-white bg-opacity-95 p-8 rounded-lg shadow-2xl w-full max-w-md"
        >
          <h2 className="text-3xl font-bold text-center text-gray-800 mb-6">
            Login to LMS
          </h2>
          {error && (
            <div className="bg-red-100 text-red-700 px-4 py-2 rounded mb-4 text-sm">
              {error}
            </div>
          )}
          <div className="mb-4">
            <label className="block text-sm text-gray-600 mb-1">Email</label>
            <input
              type="email"
              name="email"
              value={form.email}
              onChange={handleChange}
              required
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div className="mb-6">
            <label className="block text-sm text-gray-600 mb-1" />
            <input
              type="password"
              name="password"
              value={form.password}
              onChange={handleChange}
              required
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="**********"
            />
          </div>
          <button
            type="submit"
            className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded"
          >
            Login
          </button>
          <p className="mt-4 text-center text-sm text-gray-500">
            Don't have an account?{" "}
            <a href="/register" className="text-blue-600" />
          </p>
        </form>
      </div>
    </div>
  );
};
export default Login;
