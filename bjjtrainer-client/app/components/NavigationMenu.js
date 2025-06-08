"use client";
import Link from "next/link";
import { usePathname } from "next/navigation";

const navLinks = [
  { href: "/coach/dashboard", label: "Dashboard" },
  { href: "/coach/user-management", label: "User Management" },
  { href: "/coach/student-progress", label: "Student Progress" },
  { href: "/coach/school-membership", label: "School Membership" },
];

export default function NavigationMenu() {
  const pathname = usePathname();
  return (
    <nav style={{ marginBottom: "2rem", borderBottom: "1px solid #eee", paddingBottom: "1rem" }}>
      <ul style={{ display: "flex", gap: "2rem", listStyle: "none", padding: 0, margin: 0 }}>
        {navLinks.map((link) => (
          <li key={link.href}>
            <Link
              href={link.href}
              style={{
                textDecoration: pathname === link.href ? "underline" : "none",
                fontWeight: pathname === link.href ? "bold" : "normal",
                color: "#222",
              }}
            >
              {link.label}
            </Link>
          </li>
        ))}
      </ul>
    </nav>
  );
}