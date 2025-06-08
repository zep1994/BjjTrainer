"use client";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import DashboardTile from "./DashboardTile";
import styles from "./CoachDashboard.module.css";

const schoolId = 1; // Replace with dynamic value as needed
const API_BASE = "http://localhost:5057/api";

export default function CoachDashboard({ onUserManagement, onStudentProgress, onSchoolMembership }) {
  const [stats, setStats] = useState({
    totalStudents: 0,
    upcomingEvents: 0,
    attendanceRate: "93%",
    lessonPlans: 8,
    pendingPayments: 3,
  });
  const router = useRouter();

  useEffect(() => {
    fetch(`${API_BASE}/users/school/${schoolId}/students`)
        .then((res) => res.json())
        .then((data) =>
        setStats((prev) => ({ ...prev, totalStudents: data.length }))
    );

    fetch(`${API_BASE}/calendar/school/${schoolId}/upcoming-events-count`)
        .then((res) => res.json())
        .then((data) =>
        setStats((prev) => ({ ...prev, upcomingEvents: data.count }))
    );
    }, []);

  return (
    <section className={styles.dashboard}>
      <h1>School Management Dashboard</h1>
      <div className={styles.statsGrid}>
        <DashboardTile
          title="Total Students"
          value={stats.totalStudents}
          buttonText="View All Students"
          onButtonClick={() => router.push("/students")}
        />
        <DashboardTile
          title="Events This Week"
          value={stats.upcomingEvents}
          buttonText="View Events"
          onButtonClick={() => router.push("/events")}
        />
        <DashboardTile title="Attendance Rate" value={stats.attendanceRate} />
        <DashboardTile title="Lesson Plans" value={stats.lessonPlans} />
        <DashboardTile title="Pending Payments" value={stats.pendingPayments} />
      </div>
      <div style={{ display: "flex", gap: "2rem" }}>
        <DashboardTile
          title="User Management"
          value="Manage users and roles"
          buttonText="Go"
          onButtonClick={onUserManagement}
        />
        <DashboardTile
          title="Student Progress"
          value="View student stats"
          buttonText="Go"
          onButtonClick={onStudentProgress}
        />
        <DashboardTile
          title="School Membership"
          value="Manage school members"
          buttonText="Go"
          onButtonClick={onSchoolMembership}
        />
      </div>
    </section>
  );
}