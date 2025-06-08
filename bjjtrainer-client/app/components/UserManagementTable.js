"use client";
import { useEffect, useState } from "react";

export default function UserManagementTable() {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    // This code only runs on the client
    fetch("http://localhost:5057/api/coach/school/users", {
      headers: { Authorization: "Bearer " + localStorage.getItem("token") }
    })
      .then(res => res.json())
      .then(setUsers);
  }, []);

  // Add UI for changing roles/removing users as needed
  return (
    <table>
      <thead>
        <tr>
          <th>Name</th><th>Email</th><th>Role</th><th>Belt</th><th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {users.map(u => (
          <tr key={u.id}>
            <td>{u.userName}</td>
            <td>{u.email}</td>
            <td>{u.role}</td>
            <td>{u.belt}</td>
            <td>
              {/* Add buttons for role change/removal here */}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}