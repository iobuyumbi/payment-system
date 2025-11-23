import { KTIcon } from '../../../../_metronic/helpers'
import React from 'react'

interface ActionItem {
  id: string | number
  name: string
}

interface Props {
  loanBatches: ActionItem[]
  actions: ActionItem[]
  handleSearchChange: (e: React.ChangeEvent<HTMLSelectElement>) => void
  selectedStatusId: string | number
  setSelectedStatusId: (value: string) => void
  handleApplyClick: () => void
 handleLoanBatchChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
  loading: boolean
}

const SearchComponent: React.FC<Props> = ({
  actions,
  loanBatches,
  handleSearchChange,
  selectedStatusId,
  setSelectedStatusId,
  handleApplyClick,
  handleLoanBatchChange,
  loading
}) => {
  return (
    <div className="d-flex justify-content-start mb-3">
      <div className="d-flex align-items-end gap-3 flex-wrap">

        {/* Product */}
        <div className="d-flex flex-column me-3">
          <label className="form-label fw-bold mb-1">Loan Product</label>
          <select className="form-control" style={{ width: '300px' }} onChange={handleLoanBatchChange}>
            {loanBatches && loanBatches.map((item) => (
              <option key={item.id} value={item.id}>
                {item.name}
              </option>
            ))}
          </select>
        </div>

        {/* Status */}
        <div className="d-flex flex-column me-3">
          <label className="form-label fw-bold mb-1">Status</label>
          <select className="form-control" style={{ width: '200px' }} onChange={handleSearchChange}>
            {actions && actions.map((item) => (
              <option key={item.id} value={item.id}>
                {item.name}
              </option>
            ))}
          </select>
        </div>

        {/* Actions */}
        {/* <div className="d-flex flex-column me-3">
          <label className="form-label fw-bold mb-1">Status</label>
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
        </div> */}

        {/* Buttons */}
        {/* <div className="d-flex align-items-end gap-2">
          <button
            type="button"
            className="btn btn-sm btn-primary px-4 "
            onClick={handleApplyClick}
          >
            {!loading && <span className="indicator-label">Search</span>}
            {loading && (
              <span className="indicator-progress d-block">
                Please wait...
                <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
              </span>
            )}
          </button>

        </div> */}

      </div>
    </div>
  )
}

export default SearchComponent
