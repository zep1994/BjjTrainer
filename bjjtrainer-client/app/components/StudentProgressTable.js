"use client";
import { useEffect, useState } from "react";

export default function StudentProgressTable() {
  const [students, setStudents] = useState([]);
  useEffect(() => {
    fetch("http://localhost:5057/api/coach/school/students/progress", {
      headers: { Authorization: "Bearer " + localStorage.getItem("token") }
    })
      .then(res => res.json())
      .then(setStudents);
  }, []);

  return (
    <table>
      <thead>
        <tr>
          <th>Name</th><th>Belt</th><th>Total Training Time</th><th>Favorite Move</th>
        </tr>
      </thead>
      <tbody>
        {students.map(s => (
          <tr key={s.id}>
            <td>{s.userName}</td>
            <td>{s.belt}</td>
            <td>{s.progress.totalTrainingTime}</td>
            <td>{s.progress.favoriteMoveThisMonth}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}