import CustomTable from "../../../_shared/CustomTable/Index";
import { useState, useEffect } from "react";
import FarmerService from "../../../services/FarmerService";
import { Content } from "../../../_metronic/layout/components/content";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { KTCard, KTCardBody, KTIcon } from "../../../_metronic/helpers";
import { IConfirmModel } from "../../../_models/confirm-model";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { getSelectedCountryCode } from "../../../_metronic/helpers/AppUtil";
import moment from "moment";
import { Error401 } from "../../modules/errors/components/Error401";
import UserService from "../../../services/UserService";

const userService = new UserService();
const breadCrumbs: Array<PageLink> = [
    {
        title: "Dashboard",
        path: "/dashboard",
        isSeparator: false,
        isActive: true,
    },
    {
        title: "",
        path: "",
        isSeparator: true,
        isActive: true,
    },
];

export const AccessLog: React.FC = (props: any) => {

    const [rowData, setRowData] = useState<any>();
    const [stats, setStats] = useState<any>();
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [loading, setLoading] = useState(false);

    const DateComponent = (props: any) => {
        return (
            <div className="d-flex flex-row">
                {moment(props.data.accessTime).format('YYYY-MM-DD HH:mm ')}
            </div>
        );
    };

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs] = useState<any>([
        { field: "userName", flex: 1, filter: true },
        { field: "accessTime", flex: 1, filter: true, cellRenderer: DateComponent },
        { field: "ipAddress", flex: 1, filter: true },
        { field: "userAgent", flex: 1, filter: true },
    ]);

    const bindAccessLog = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const data: any = {
                pageNumber: 1,
                pageSize: 100,
            };

            const response = await userService.getAccessLog(data);
            console.log(response)
            setRowData(response ? response : []);
        }
    };

    const countryCode = getSelectedCountryCode()

    useEffect(() => {
        bindAccessLog();
    }, [countryCode]);

    return (
        <Content>
            {isAllowed("logs.audit") ? (
                <>
                    {" "}
                    <PageTitleWrapper />
                    <PageTitle breadcrumbs={breadCrumbs}>System Access Log</PageTitle>
                    <KTCard className="shadow my-3">
                        <KTCardBody className="m-0 p-0">
                            <CustomTable
                                rowData={rowData}
                                colDefs={colDefs}
                                header=""
                                addBtnText={""}
                                searchTerm={searchTerm}
                                setSearchTerm={setSearchTerm}
                                addBtnLink={""}
                                showExport={false}
                            />
                        </KTCardBody>
                    </KTCard>{" "}
                </>
            ) : (
                <Error401 />
            )}
        </Content>
    );
};
