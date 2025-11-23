import React from 'react';

export type Status = 'Draft' | 'Accepted' | 'Rejected' | 'Disbursed' | 'Closed';

export const statusTransitions: Record<Status, Status[]> = {
    Draft: ['Accepted', 'Rejected'],
    Accepted: ['Disbursed'],
    Rejected: [],
    Disbursed: ['Closed'],
    Closed: [],
};

const StatusTransitionGuide: React.FC = () => {
    return (
        <div style={{
            border: '1px solid #ccc',
            borderRadius: '6px',
            padding: '1rem',
            backgroundColor: '#f9f9f9',
            fontSize: '0.9rem',
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))',
            gap: '0.5rem',
            alignItems: 'start'
        }}>
            <strong style={{ gridColumn: '1 / -1', marginBottom: '0.5rem' }}>Status Transition Guide:</strong>
            {Object.entries(statusTransitions).map(([fromStatus, toStatuses]) => (
                <div key={fromStatus}>
                    <span style={{ fontWeight: 500 }}>{fromStatus}</span> →{' '}
                    {toStatuses.length > 0 ? toStatuses.join(', ') : '(No transitions)'}
                </div>
            ))}
        </div>
    );
};

export default StatusTransitionGuide;
