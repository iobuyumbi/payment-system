import React, { useState } from 'react';
import classnames from 'classnames';

import './pagination.scss';
import Select from "react-select";
import { Row , Col, Form} from 'react-bootstrap';
import { debounce } from 'lodash';
import styled from '@emotion/styled';
import { usePagination } from './usePagination';

const PaginationNew = (props: any) => {
  const { onPageChange, totalCount, siblingCount = 1, currentPage, pageSize, className, onPageCountChange, pageCountArr } = props;

  const paginationRange: any = usePagination({ currentPage, totalCount, siblingCount, pageSize });
  const [emptyInput, setEmptyInput] = useState<number | string>(0);
  const [isPageInputActive, setIsPageInputActive] = useState<boolean>(false);

  if (currentPage === 0 || paginationRange.length === 0 ) {
    return null;
  }

  const onNext = () => {
    onPageChange(currentPage + 1);
    document?.getElementById('commonTable')?.scrollIntoView({
      behavior: 'smooth'
    });
  };

  const onPrevious = () => {
    onPageChange(currentPage - 1);
    document?.getElementById('commonTable')?.scrollIntoView({
      behavior: 'smooth'
    });
  };

  const handlePageInput = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;

    let pageNumber = +value > 0 ? +value : 1;
    if(pageNumber > lastPage){
        pageNumber = lastPage;
    }

    const optimizedOnPageChange = debounce(() => {
      if(value === ''){
        setEmptyInput("");
      }else{
        setEmptyInput(0);
      }
      
      onPageChange(pageNumber)
    }, 100);

    optimizedOnPageChange();
};

const handlePageInputActive = () => {
  setIsPageInputActive(true);
}

const handlePageInputExit = () => {
  setIsPageInputActive(false);
  setEmptyInput(0);
};

  let lastPage = paginationRange[paginationRange.length - 1];
  return (
    <Div>
      <Row className="d-flex align-items-center m-0 px-3">
        <Col className="pr-0 col-auto pr-2">
              <span>Showing {(currentPage-1)*pageSize+1}-{currentPage === lastPage ? totalCount : pageSize*currentPage} of {totalCount}</span>
          </Col>
        <Col className="pr-0 col-auto pr-2 align-items-center d-flex flex-grow-1">
            <span className="mx-2">Items per page</span>
            <span className="select-parent my-2">
              <Select
                  menuPlacement="top"
                  placeholder="Select Pagecount"
                  onChange={onPageCountChange}
                  value={
                    pageCountArr.length > 0 &&
                    pageCountArr.filter(
                      (option: any) => option.value == pageSize
                    )
                  }
                  options={pageCountArr}
                />
            </span>
        </Col>
        <Col className="pr-0 col-auto pr-2">
            {totalCount &&
            <ul
                className={classnames('pagination-container', { [className]: className })}
                >
                <div className='mt-3 align-items-center d-flex'>
                  <span>Page</span> 
                  <Form.Control
                      type="number"
                      size="sm"
                      className='bootstrap-number-field'
                      value={emptyInput === "" && isPageInputActive ? emptyInput : currentPage}
                      onChange={handlePageInput}
                      onClick={handlePageInputActive}
                      onBlur={handlePageInputExit}
                  />
                  <span>of {lastPage}</span>
                </div>
                <li
                    className={classnames('pagination-item', {
                    disabled: currentPage === 1
                    })}
                    onClick={onPrevious}
                    style={{marginTop:"17px"}}
                >
                    <div className="arrow left fa-2x" style={{ transform: 'translateY(1px) rotate(-135deg)' }} />
                </li>
                <li
                    className={classnames('pagination-item', {
                    disabled: currentPage === lastPage
                    })}
                    onClick={onNext}
                    style={{marginTop:"13px"}}
                >
                    <div className="arrow right fa-2x" />
                </li>
                </ul>}
          </Col>
      </Row>
    </Div>
  );
}

const Div = styled.div`
    .bootstrap-number-field { 
      height: 38px;
      width: 85px;
      margin: 0 10px;
      font-size: 1rem;
      padding: 8px !important;

      &:focus-visible{
        border: 2px solid #015ECC !important;
      }
    }
`;

export default PaginationNew;