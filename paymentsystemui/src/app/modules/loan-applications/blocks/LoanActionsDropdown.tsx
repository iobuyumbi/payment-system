import React from 'react';

interface Props {
  loanBatch: any,
  handleImportApplication: () => void,
  handleCreateNewLoan: () => void
}
const LoanActionsDropdown: React.FC<Props> = ({ loanBatch, handleImportApplication, handleCreateNewLoan }) => {

  return (
    <div className="dropdown">
      <button
        className="btn btn-sm btn-secondary dropdown-toggle"
        type="button"
        id="loanActionsDropdown"
        data-bs-toggle="dropdown"
        aria-expanded="false"
      >
        More
      </button>
      <ul className="dropdown-menu" aria-labelledby="loanActionsDropdown">
        {/* <li>
          <button className="dropdown-item" onClick={handleCreateNewLoan}>
            Create New Loan
          </button>
        </li> */}
        <li>
          <button className="dropdown-item" onClick={handleImportApplication}>
            Import Applications
          </button>
        </li>
      </ul>
    </div>
  );
};

export default LoanActionsDropdown;
