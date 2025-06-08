"use client";
import { useEffect, useState } from "react";

export default function StudentsPage() {
  const [students, setStudents] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Replace with your actual schoolId and API URL
    const schoolId = 1;
    fetch(`http://localhost:5000/api/schools/${schoolId}/students`)
      .then((res) => res.json())
      .then((data) => {
        setStudents(data);
        setLoading(false);
      });
  }, []);

  if (loading) return <p>Loading...</p>;

  return (
    <section>
      <h1>All Students</h1>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            {/* Add more columns as needed */}
          </tr>
        </thead>
        <tbody>
          {students.map((student) => (
            <tr key={student.id}>
              <td>{student.userName}</td>
              <td>{student.email}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}