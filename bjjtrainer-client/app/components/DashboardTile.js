export default function DashboardTile({ title, value, buttonText, onButtonClick }) {
  return (
    <div className="dashboard-card">
      <h2>{title}</h2>
      <p>{value}</p>
      {buttonText && onButtonClick && (
        <button onClick={onButtonClick}>{buttonText}</button>
      )}
    </div>
  );
}