import { useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { useParams } from "react-router-dom";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import LoanTermsForm from "./partials/LoanTermsForm";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";


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
    isActive: false,
  },
  {
    title: "Master Loan Terms",
    path: "/master-loan-terms",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  },
];

export function AddMasterLoanTerms() {
  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add" : "Edit");

  return (
    <Content>
      {isAllowed("loans.batch.add") ? (
        <>
          {" "}
          <PageTitleWrapper />
          <PageTitle breadcrumbs={breadCrumbs}>{title} master terms</PageTitle>
          <KTCard className="my-3">
            <KTCardBody>
              <LoanTermsForm title={title} />
            </KTCardBody>
          </KTCard>

        </>
      ) : (
        <Error401 />
      )}
    </Content>
  );
}
