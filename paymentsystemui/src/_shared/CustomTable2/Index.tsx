/* eslint-disable no-var */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { Card } from "react-bootstrap";
import { useCallback, useEffect, useState } from "react";
import { TableBodyNew } from "./TableBodyNew";
import { TableHeader } from "./TableHeader";
import { ProjectsListHeader } from "./projects/ProjectsListHeader";
import { FarmersListHeader } from "./farmers/FarmersListHeader";

const CustomTable = (props: any) => {
    const { headerTitle, getTableBodyData, getTableDataService, deleteTableDataService, setAction, 
        tablePhase, 
        tableHeaderArr,
        addLink, 
        addNewBtnText
     } = props;
    const [tableData, setTableData] = useState<any>([]);
    const [currentPage, setCurrentPage] = useState<number>(1);
    const [pageSize, setPageSize] = useState<number>(20);
    const [totalRows, setTotalRows] = useState<number>(0);
    const [multipleRowData, setMultipleRowData] = useState<any>([]);
    const [editMultipleFlag, setEditMultipleFlag] = useState<any>(false);
    const [checkedState, setCheckedState] = useState<any>([]);


    const getTableData = useCallback(() => {
        getTableDataService().then((data: any) => {
            if (data && data.length > 0) {
                setTotalRows(data.length);
                setTableData(data);

                if (tablePhase === 1) {
                    var arrayCount = 0;
                    data.map((option: any) => {
                        option.data.map((dataOption: any) => {
                            arrayCount += 1;
                            dataOption['selectedIndex'] = arrayCount - 1;
                        });
                    });
                    setCheckedState(new Array(arrayCount).fill(false));
                } else {
                    setCheckedState(new Array(data.length).fill(false));
                }
            } else {
                setTableData([]);
            }
        });
    }, [getTableDataService, tablePhase]);

    useEffect(() => {
        getTableData();
    }, [getTableData]);

    return (
        <Card className="mt-2">
            <Card.Body className="p-0">
                <TableHeader
                />
                <TableBodyNew
                    {...props}
                    setAction={setAction}
                    getTableData={getTableData}
                    tableData={tableData}
                    tableOtherData={tableData}
                    editMultipleFlag={editMultipleFlag}
                    setEditMultipleFlag={setEditMultipleFlag}
                    setCheckedState={setCheckedState}
                    checkedState={checkedState}
                    setMultipleRowData={setMultipleRowData}
                    multipleRowData={multipleRowData}
                    deleteTableDataService={deleteTableDataService}
                />
            </Card.Body>
        </Card>
    );
};

export default CustomTable;
