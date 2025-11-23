import { KTIcon } from '../../../../_metronic/helpers'
import React from 'react'
import LoanActionsDropdown from './LoanActionsDropdown'

interface ActionItem {
  id: string | number
  name: string
}

interface Props {
  actions: ActionItem[]
  handleSearchChange: (e: React.ChangeEvent<HTMLSelectElement>) => void
  selectedStatusId: string | number
  setSelectedStatusId: (value: string) => void
  handleApplyClick: () => void
  handleExcelExport: () => void
  loading: boolean
  loanBatch: any
  handleImportApplication: () => void
  handleCreateNewLoan: () => void
}

const ActionComponent: React.FC<Props> = ({
  actions,
  handleSearchChange,
  selectedStatusId,
  setSelectedStatusId,
  handleApplyClick,
  handleExcelExport,
  loading,
  loanBatch,
  handleImportApplication,
  handleCreateNewLoan
}) => {
  return (
    <div className="d-flex justify-content-end my-3">
      <div className="d-flex align-items-end gap-3 flex-wrap">

        {/* Actions */}
        <div className="d-flex flex-row me-3">
          {/* <label className="form-label fw-bold mb-1">Status</label> */}
          <select
            className="form-control"
            style={{ width: '200px' }}
            value={selectedStatusId}
            onChange={(e) => setSelectedStatusId(e.target.value)}
          >
            {actions && actions.map((item) => (
              <option key={item.id} value={item.id}>
                {item.name}
              </option>
            ))}
          </select>
        </div>

        {/* Buttons */}
        <div className="d-flex align-items-end gap-2">
          <button
            type="button"
            className="btn btn-sm btn-theme px-4"
            onClick={handleApplyClick}
          >
            {!loading && <span className="indicator-label">Update Status</span>}
            {loading && (
              <span className="indicator-progress d-block">
                Please wait...
                <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
              </span>
            )}
          </button>
          <button className="btn btn-sm btn-secondary " onClick={handleExcelExport}>
            <KTIcon iconName="exit-down" /> Download Excel
          </button>

          <LoanActionsDropdown
            loanBatch={loanBatch}
            handleImportApplication={handleImportApplication}
            handleCreateNewLoan={handleCreateNewLoan}
          />
        </div>

      </div>
    </div>
  )
}

export default ActionComponent
