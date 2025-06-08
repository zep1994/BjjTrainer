import Image from "next/image";
import styles from "./page.module.css";
import CoachDashboard from "./components/CoachDashboard";

export default function Home() {
  // In the future, you can check user role here and render different dashboards
  return <CoachDashboard />;
}
  