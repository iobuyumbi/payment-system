/* eslint-disable no-var */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { useEffect, useState, useCallback } from "react";
import { Col, Row, Form, Button } from "react-bootstrap";
import "./tableStyle.scss";
import { removeItemAll } from "../../_metronic/helpers/UIUtil";
import { PAGE_COUNT_ARR, toAbsoluteUrl } from "../../_metronic/helpers";
import PaginationNew from "../Pagination/PaginationNew";
import { FaArrowAltCircleDown, FaArrowAltCircleUp } from "react-icons/fa";
import * as _ from "lodash";

const PAGE_SIZE = 10;

export const TableBodyNew: React.FC = (props: any) => {
    const {
        tableHeaderArr,
        openAddEditPopup,
        getTableBodyData,
        tableData,
        showPopUp,
        multipleRowData,
        setMultipleRowData,
        tablePhase,
        setCheckedState,
        checkedState,
        deleteTableDataService,
        addNewBtnText,
        showdataCompleteness,
        editTop,
        groupHeadingKey,
        openReplcatePopup,
        editMultipleFlag,
        selectedMappinIDs,
        setSelectedMappinIDs,
        deleteMultipleFlag,
        groupField,
        groupFieldKey,
        replicateFlag
    } = props;

    const [tableLocalData, setTableLocalData] = useState<any>([]);
    const [confirm, setConfirm] = React.useState<any | null>(null);
    const [currentPage, setCurrentPage] = React.useState<number>(1);
    const [pageSize, setPageSize] = React.useState<number>(PAGE_SIZE);
    const [localTableHeaderArr, setLocalTableHeaderArr] = useState<any>([]);
    const currentPermission: any = [];

    useEffect(() => {
        const currentPaging = (currentPage <= Math.ceil(tableData.length / pageSize)) ? currentPage : 1;
        paginate(currentPaging)
    }, [tableData, pageSize]);


    useEffect(() => {
        setLocalTableHeaderArr(tableHeaderArr);
    }, [tableHeaderArr]);

    const handleOnChange = (e: any, data: any) => {
        let tempSelectedMappinIDs = [...selectedMappinIDs];
        if (groupField) {
            if (!tempSelectedMappinIDs.includes(data?.mappingId)) {
                tempSelectedMappinIDs.push(data?.mappingId)
            }
            else {
                tempSelectedMappinIDs = removeItemAll(tempSelectedMappinIDs, data?.mappingId)
            }
            setSelectedMappinIDs(tempSelectedMappinIDs)

        } else {
            if (!tempSelectedMappinIDs.includes(data?.mappingId)) {
                tempSelectedMappinIDs.push(data?.mappingId)
            }
            else {
                tempSelectedMappinIDs = removeItemAll(tempSelectedMappinIDs, data?.mappingId)
            }
            setSelectedMappinIDs(tempSelectedMappinIDs)
        }

        storeMultipleRowData(e, data);
    };

    const storeMultipleRowData = useCallback((e: any, data: any) => {
        var localArr = multipleRowData.filter(
            (option: any) => option.mappingId != data.mappingId
        );
        if (e.target.checked) {
            setMultipleRowData([...multipleRowData, data]);
        } else {
            setMultipleRowData(localArr);
        }
    }, [multipleRowData]);

    const deleteTableRow = (currenRowow: any) => {
        setConfirm({
            type: "ConfirmType.DELETE",
            title: "Delete records",
            message: `Are you sure you want to delete the ${addNewBtnText}?`,
            show: true,
            okCallback: (args: React.MouseEvent) => {
                if (groupField) {
                    deleteTableDataService({
                        fieldId: currenRowow?.fieldId,
                        harvestYear: currenRowow?.harvestYear
                    }).then((data: any) => {
                        onCloseConfirm();
                    });
                } else {
                    deleteTableDataService(currenRowow.mappingId).then((data: any) => {
                        onCloseConfirm();
                    });
                }
            },
            cancelCallback: onCloseConfirm,
        });
    };

    const deleteTotal = (currentRow: any) => {
        setConfirm({
            type: "ConfirmType.DELETE",
            title: "Delete records",
            message: `Are you sure you want to delete the data for this field and year?`,
            show: true,
            okCallback: (args: React.MouseEvent) => {
                deleteTableDataService(currentRow).then((data: any) => {
                    onCloseConfirm();
                });
            },
            cancelCallback: onCloseConfirm,
        });
    };

    const onCloseConfirm = () => {
        setConfirm(null);
    };

    const editField = (action: any, data: any, index: any) => {
        // if data contains a sub row, then pass the data of sub row based on index otherwise pass data
        const currentRowData = data?.data?.[index] || data;
        if (tablePhase == 2) {
            openAddEditPopup(action, currentRowData);
        } else {
            openAddEditPopup(action, currentRowData);
        }
    };

    const editTotalField = (action: any, x: any, index: any) => {
        if (tablePhase == 1) {
            openAddEditPopup(action, x);
        }
    };

    const onPageCountChange = (e: any) => {
        setCurrentPage(1);
        setPageSize(parseInt(e.value));
    };

    const replcateField = (action: any, x: any, index: any) => {
        if (tablePhase == 2) {
            openReplcatePopup(action, x);

        } else {
            openReplcatePopup(action, groupField ? x : x.data[index]);
        }

    }

    const actionColumn = (editObj: any, deleteObj: any, editIndex: any) => {
        return (
            <>
                {
                    replicateFlag ?
                        <Col>
                            <Button variant="success" onClick={() => { replcateField("replicate", editObj, editIndex); }}>Select</Button>
                        </Col>
                        : <Col className={showdataCompleteness == true ? "text-left" : "text-left max-80"}>
                            <>
                                {!editMultipleFlag && !deleteMultipleFlag && (
                                    <>

                                        <img
                                            src={toAbsoluteUrl("media/icons/duotune/abstract/edit.svg")}
                                            width="22"
                                            className="mx-3"
                                            style={{ cursor: "pointer", pointerEvents: deleteMultipleFlag ? 'none' : 'auto' }}
                                            onClick={() => {
                                                editField("edit", editObj, editIndex);
                                            }}
                                        />
                                        <img
                                            src={toAbsoluteUrl("media/icons/duotune/abstract/delete.svg")}
                                            width="12"
                                            className="mx-3"
                                            style={{ cursor: currentPermission.isDelete ? "pointer" : "default", pointerEvents: deleteMultipleFlag ? 'none' : 'auto' }}
                                            onClick={() => {
                                                currentPermission.isDelete && deleteTableRow(deleteObj);
                                            }}
                                        />

                                    </>
                                )}
                            </>
                        </Col>
                }
            </>
        );
    };


    const paginate = (pageNumber: number) => {
        setCurrentPage(pageNumber);
        const firstPageIndex = (pageNumber - 1) * pageSize;
        const lastPageIndex = firstPageIndex + pageSize;
        var localArr = tableData.slice(firstPageIndex, lastPageIndex);
        setTableLocalData(localArr);
    };

    const [sortConfig, setSortConfig] = useState<any>({ key: 'projectName', direction: 'asc' });

    const sortTableData = () => {
        const { key, direction } = sortConfig;
        const sortedData = _.orderBy(tableLocalData, [key], [direction]);

        setTableLocalData(sortedData);
        setSortConfig({
            key,
            direction: direction === 'asc' ? 'desc' : 'asc',
        });
    };

    const resetTableHeaderArr = (index: any, sortDir: any) => {
        var localArr: any = []
        localTableHeaderArr.map((option: any, optionIndex: any) => {
            if (optionIndex == (index - 1)) {
                localArr.push({ ...option, sortDir: sortDir, sortVisibleFlag: true })
            } else {

                localArr.push({ ...option, sortVisibleFlag: false })
            }
        })
        setLocalTableHeaderArr(localArr)

    }

    const getGroupData = (colData: any, groupByKeyValue: any) => {
        return (
            <>
                {colData?.data?.length > 0 && colData?.data?.map((data: any, innerIndex: any) => {
                    return (
                        <Row
                            className="d-flex m-0 border-bottom py-4 m-0 selectall-td selectcolumn"
                            key={Math.random()}
                        >
                            {(editMultipleFlag) && (
                                <Col className="select-firsttd px-3">
                                    <Form.Group controlId="formBasicCheckbox">
                                        <Form.Check
                                            type="checkbox"
                                            className="mt-0"
                                            checked={
                                                selectedMappinIDs.includes(data.mappingId)
                                            }
                                            label=""
                                            onChange={(e: any) => {
                                                // storeMultipleRowData(e, obj);
                                                handleOnChange(e, data);

                                            }}
                                        />
                                    </Form.Group>
                                </Col>
                            )}
                            {(deleteMultipleFlag) && (
                                <Col className="select-firsttd px-3">
                                    <Form.Group controlId="formBasicCheckbox">
                                        <Form.Check
                                            type="checkbox"
                                            className="mt-0"
                                            checked={
                                                groupField ? selectedMappinIDs.includes(data.mappingId) : selectedMappinIDs.includes(data?.mappingId)
                                            }
                                            label=""
                                            onChange={(e: any) => {
                                                handleOnChange(data.mappingId, data);
                                            }}
                                        />
                                    </Form.Group>
                                </Col>
                            )}


                            {getTableBodyData(data)}
                            {actionColumn(data, data, innerIndex)}

                        </Row>
                    );
                })}
            </>
        )
    };
    return (
        <>
            <Col className="pt-10 table-responsive" >
                <Row className="d-flex table-header m-0">

                    {localTableHeaderArr && localTableHeaderArr.map((option: any, i: any) => {
                        return (
                            <React.Fragment key={Math.random()}>
                                {i == 0 ? (
                                    editMultipleFlag ? (
                                        <Col
                                            className={"border-bottom py-4 px-4 select-firstth"}
                                        ></Col>
                                    )
                                        : null
                                )
                                    : editMultipleFlag && deleteMultipleFlag && option?.editMultipleVisibleFlag !== undefined && !option?.editMultipleVisibleFlag ?
                                        <Col className={`sortIconWrapper border-bottom py-4 th-header text-break columngrid-align ${i == localTableHeaderArr.length - 1 ? showdataCompleteness == true ? "" : replicateFlag ? "" : "text-left max-80" : "text-left"}`}></Col>
                                        : (
                                            <Col
                                                className={`sortIconWrapper border-bottom py-4 th-header text-break columngrid-align ${i == localTableHeaderArr.length - 1
                                                    ? showdataCompleteness == true ? "" : replicateFlag ? "" : "text-left max-80"
                                                    : "text-left"
                                                    }`
                                                }
                                                onClick={() => {
                                                    resetTableHeaderArr(i + 1, !option.sortDir)
                                                }}
                                            >

                                                <span className="icon-text" onClick={() => sortTableData()}>{option.label} {option?.info && <></>}
                                                    <span title="Sort" className={`cursor icon ${option.sortVisibleFlag ? "open" : ""}`}>{option.sortLabel != null ? option.sortDir ? <FaArrowAltCircleDown /> : <FaArrowAltCircleUp /> : null}</span>
                                                </span>
                                            </Col>
                                        )}
                            </React.Fragment>
                        );
                    })}
                </Row>

                <Row className="d-flex m-0">
                    <Col md={12} className="p-0">
                        {tableLocalData?.length === 0 ? (
                            <Row
                                className="d-flex border-bottom py-4 px-15 m-0 font-weight-bold w-100"
                                style={{
                                    textAlign: "center",
                                }}
                            >
                                <Col className="px-0 text-muted">No records are present</Col>
                            </Row>
                        ) : (

                            tableLocalData?.length > 0 &&
                            tableLocalData.map((x: any, index: any) => {
                                return (
                                    <React.Fragment key={Math.random()}>
                                        {tablePhase == 1 ? (
                                            <>
                                                <Row className="d-flex border-bottom py-4 px-15 m-0 font-weight-bold body-column-section">
                                                    <Col className="px-0 translation-exclusions">{x[groupHeadingKey]}</Col>
                                                    {editTop && (
                                                        <Col className="text-right">
                                                            <img
                                                                src={toAbsoluteUrl("media/icons/duotune/abstract/delete.svg")}
                                                                width="12"
                                                                className="mx-3"
                                                                style={{ cursor: "pointer" }}
                                                                onClick={() => {
                                                                    deleteTotal(x);
                                                                }}
                                                            />
                                                            <img
                                                                src={toAbsoluteUrl("media/icons/duotune/abstract/edit.svg")}
                                                                width="22"
                                                                className="mx-3"
                                                                style={{ cursor: "pointer" }}
                                                                onClick={() => {
                                                                    editTotalField("edit", x, index);

                                                                }}
                                                            />{" "}
                                                        </Col>
                                                    )}
                                                </Row>
                                                <Row className="m-0">
                                                    <Col className="p-0">
                                                        {groupField ? getGroupData(x, groupFieldKey) : x.data.map((obj: any, innerIndex: any) => {
                                                            return (
                                                                <Row
                                                                    className="d-flex m-0 border-bottom py-4 m-0 selectall-td selectcolumn"
                                                                    key={Math.random()}
                                                                >
                                                                    {(editMultipleFlag) && (
                                                                        <Col className="select-firsttd px-3">
                                                                            <Form.Group controlId="formBasicCheckbox">
                                                                                <Form.Check
                                                                                    type="checkbox"
                                                                                    className="mt-0"
                                                                                    checked={
                                                                                        selectedMappinIDs.includes(obj.mappingId)
                                                                                    }
                                                                                    label=""
                                                                                    onChange={(e: any) => {
                                                                                        // storeMultipleRowData(e, obj);
                                                                                        handleOnChange(e, obj);

                                                                                    }}
                                                                                />
                                                                            </Form.Group>
                                                                        </Col>
                                                                    )}
                                                                    {(deleteMultipleFlag) && (
                                                                        <Col className="select-firsttd px-3">
                                                                            <Form.Group controlId="formBasicCheckbox">
                                                                                <Form.Check
                                                                                    type="checkbox"
                                                                                    className="mt-0"
                                                                                    checked={
                                                                                        selectedMappinIDs.includes(obj.mappingId)
                                                                                    }
                                                                                    label=""
                                                                                    onChange={(e: any) => {
                                                                                        // handleOnChange(obj.mappingId);
                                                                                        handleOnChange(e, obj);


                                                                                    }}
                                                                                />
                                                                            </Form.Group>
                                                                        </Col>
                                                                    )}
                                                                    {getTableBodyData(obj)}
                                                                    {actionColumn(x, obj, innerIndex)}
                                                                </Row>
                                                            );
                                                        })}
                                                    </Col>
                                                </Row>
                                            </>
                                        ) : (                                           
                                            <Row className="d-flex m-0 border-bottom py-4 px-0">
                                                {(editMultipleFlag) && (
                                                    <Col className="select-firsttd px-3">
                                                        <Form.Group controlId="formBasicCheckbox">
                                                            <Form.Check
                                                                type="checkbox"
                                                                className="mt-0"
                                                                checked={selectedMappinIDs.includes(x.mappingId)}
                                                                label=""
                                                                onChange={(e: any) => {
                                                                    handleOnChange(e, x);
                                                                }}
                                                            />
                                                        </Form.Group>
                                                    </Col>
                                                )}
                                                {(deleteMultipleFlag) && (
                                                    <Col className="select-firsttd px-3">
                                                        <Form.Group controlId="formBasicCheckbox">
                                                            <Form.Check
                                                                type="checkbox"
                                                                className="mt-0"
                                                                checked={selectedMappinIDs.includes(x.mappingId)}
                                                                label=""
                                                                onChange={(e: any) => {
                                                                    handleOnChange(e, x);
                                                                }}
                                                            />
                                                        </Form.Group>
                                                    </Col>
                                                )}
                                                {getTableBodyData(x)}
                                                {actionColumn(x, x, index)}
                                            </Row>
                                        )}
                                    </React.Fragment>
                                )
                            })
                        )}
                    </Col>
                </Row>
                {tableData.length > 5 && (
                    <PaginationNew
                        className="pagination-bar"
                        currentPage={currentPage}
                        totalCount={tableData.length}
                        pageSize={pageSize}
                        onPageChange={paginate}
                        onPageCountChange={onPageCountChange}
                        pageCountArr={PAGE_COUNT_ARR}
                    />
                )}
            </Col>
        </>
    );
};
