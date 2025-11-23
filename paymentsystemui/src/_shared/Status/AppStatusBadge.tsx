import React from "react";

interface AppStatusBadgeProps {
    value: string;
}

const statusMap: Record<string, { label: string; className: string }> = {
    "Draft": { label: "Draft", className: "badge bg-secondary" },
    "Accepted": { label: "Accepted", className: "badge bg-light-primary" },
    "Rejected": { label: "Rejected", className: "badge bg-light-danger" },
    "Closed": { label: "Closed", className: "badge bg-light-info" },
    "Disbursed": { label: "Disbursed", className: "badge bg-light-success" },
};

export const AppStatusBadge: React.FC<AppStatusBadgeProps> = ({ value }) => {
    const status = statusMap[value];

    if (!status) return null;

    return <span className={status.className}>{status.label}</span>;
};
