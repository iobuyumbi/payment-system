import { Column, useTable, useSortBy, usePagination } from 'react-table';

const columns: Column<any>[] = [
    {
        Header: "ID", accessor: "id"
    },
    {
        Header: "Gender", accessor: "gender"
    },
    {
        Header: "Salary", accessor: "salary"
    }
];

import {data} from '../../_metronic/assets/data,.json';


export default function CustomTable2() {
    const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } = useTable({
        columns, data,
    },
        useSortBy,
        usePagination
    );

    const props = getTableProps();

    return (
        <div className='container'>
            <table {...props}>
                <thead>
                    {headerGroups.map((hg) => (
                        <tr {...hg.getHeaderGroupProps()}>
                            {
                                hg.headers.map((column: any) => (
                                    <th {...column.getHeaderProps(column.getSortByToggleProps())}>
                                        {column.render("Header")}
                                        {
                                            column.isSorted &&
                                            (
                                                <span>{column.isSortedDesc ? " ⬇️ " : " ⬆️ "}</span>
                                            )
                                        }
                                    </th>
                                ))
                            }
                        </tr>
                    ))}
                </thead>
                <tbody {...getTableBodyProps()}>
                    {rows.map(row => {
                        prepareRow(row)

                        return <tr {...row.getRowProps()}>
                            {row.cells.map(cell => (
                                <td {...cell.getCellProps()}>{cell.render("Cell")}</td>
                            ))}
                        </tr>
                    })}
                </tbody>
            </table>
        </div>
    )
}
