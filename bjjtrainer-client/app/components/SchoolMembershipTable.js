"use client";
import { useEffect, useState } from "react";

export default function SchoolMembershipTable() {
  const [members, setMembers] = useState([]);
  useEffect(() => {
    fetch("http://localhost:5057/api/coach/school/users", {
      headers: { Authorization: "Bearer " + localStorage.getItem("token") }
    })
      .then(res => res.json())
      .then(setMembers);
  }, []);

  // Add UI for adding/removing users as needed
  return (
    <table>
      <thead>
        <tr>
          <th>Name</th><th>Email</th><th>Role</th><th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {members.map(m => (
          <tr key={m.id}>
            <td>{m.userName}</td>
            <td>{m.email}</td>
            <td>{m.role}</td>
            <td>
              {/* Add buttons for add/remove here */}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}