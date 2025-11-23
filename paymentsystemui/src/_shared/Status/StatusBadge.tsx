import React from "react";

interface StatusBadgeProps {
  value: number;
}

const statusMap: Record<number, { label: string; className: string }> = {
  1: { label: "Draft", className: "badge bg-secondary" },
  2: { label: "In Review", className: "badge bg-light-warning" },
  3: { label: "Accepting Applications", className: "badge bg-light-success" },
  4: { label: "Closed", className: "badge bg-light-info" },
};

export const StatusBadge: React.FC<StatusBadgeProps> = ({ value }) => {
  const status = statusMap[value];

  if (!status) return null;

  return <span className={status.className}>{status.label}</span>;
};
