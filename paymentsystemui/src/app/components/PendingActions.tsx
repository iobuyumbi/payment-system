import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import clsx from "clsx";
import { KTIcon } from "../../_metronic/helpers";
import ExportService from "../../services/ExportService";
import PaymentBatchService from "../../services/PaymentBatchService";
import { isAllowed } from "../../_metronic/helpers/ApiUtil";
import { getAuth } from "../modules/auth";

const auth = getAuth();
const currentUserId = auth?.userId;
const paymentBatchService = new PaymentBatchService();
const statusContent = {
  Initiated: {
    initiator: {
      title: "Batch Initiated",
      description: "Payment batch initiated. You can now send it for review.",
      buttonText: "Send for Review",
      buttonVisible: true,
      permission: "payments.batch.add",
      popupMessage: "confirm-stage-review",
      action: "",
    },
    reviewer: {
      title: "Batch Initiated (Read-Only)",
      description:
        "You can only view this batch. No changes are allowed at this stage.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.review",
      popupMessage: "confirm-stage-approval",
      action: "",
    },
    approver: {
      title: "Batch Initiated (Read-Only)",
      description:
        "You can only view this batch. No changes are allowed at this stage.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.approve",
      popupMessage: "confirm-stage-review",
      action: "",
    },
  },
  "Under Review": {
    initiator: {
      title: "Under Review",
      description:
        "Your batch is under review. No edits are allowed at this stage.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.add",
      popupMessage: "confirm-stage-review",
      action: "",
    },
    reviewer: {
      title: "Review Pending",
      description: "You can review and approve/reject the batch.",
      buttonText: "Start Review",
      buttonVisible: true,
      permission: "payments.batch.review",
      popupMessage: "confirm-stage-approval",
      action: "start-review",
    },
    approver: {
      title: "Batch Initiated (Read-Only)",
      description:
        "You can only view this batch. No changes are allowed at this stage.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.approve",
      popupMessage: "confirm-stage-review",
      action: "",
    },
  },
  "Pending Approval": {
    initiator: {
      title: "Pending Approval",
      description:
        "Your batch is pending approval. No edits are allowed at this stage.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.add",
      popupMessage: "confirm-stage-review",
      action: "",
    },
    reviewer: {
      title: "Pending Approval",
      description:
        "Your batch is pending approval. No edits are allowed at this stage.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.review",
      popupMessage: "",
      action: "start-review",
    },
    approver: {
      title: "Pending Approval",
      description: "You can review and approve/reject the batch.",
      buttonText: "Approve Batch",
      buttonVisible: true,
      permission: "payments.batch.approve",
      popupMessage: "confirm-stage-approval",
      action: "approve",
    },
  },
  Approved: {
    initiator: {
      title: "Batch Approved",
      description: "The batch has been approved and is ready for processing.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.add",
      popupMessage: "confirm-stage-review",
      action: "",
    },
    reviewer: {
      title: "Batch Approved",
      description:
        "The batch has been approved and no further actions are needed.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.review",
      popupMessage: "confirm-stage-approval",
      action: "",
    },
    approver: {
      title: "Batch Approved",
      description:
        "The batch has been approved and processed. No further actions are needed.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.approve",
      popupMessage: "confirm-stage-review",
      action: "",
    },
  },
  Rejected: {
    initiator: {
      title: "Batch Rejected",
      description: "Please re-upload the data and resubmit for review.",
      buttonText: "Re-Upload Data",
      buttonVisible: true,
      permission: "payments.batch.add",
      popupMessage: "confirm-stage-review",
      action: "",
    },
    reviewer: {
      title: "Batch Rejected",
      description:
        "This batch was rejected. Await re-upload from the initiator.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.review",
      popupMessage: "confirm-stage-approval",
      action: "",
    },
    approver: {
      title: "Batch Rejected",
      description:
        "This batch was rejected. Await re-upload from the initiator.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.approve",
      popupMessage: "confirm-stage-review",
      action: "",
    },
  },
  "Review Rejected": {
    initiator: {
      title: "Review Rejected",
      description: "Please make corrections and start afresh for review.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.add",
      popupMessage: "confirm-stage-review",
      action: "",
    },
    reviewer: {
      title: "Review Rejected",
      description:
        "Batch review was rejected. Await a new submission from the initiator.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.review",
      popupMessage: "confirm-stage-approval",
      action: "",
    },
    approver: {
      title: "Review Rejected",
      description:
        "Batch review was rejected. Await a new submission from the initiator.",
      buttonText: "",
      buttonVisible: false,
      permission: "payments.batch.approve",
      popupMessage: "confirm-stage-review",
      action: "",
    },
  },
};

export default function PendingActions() {
  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<string>("");

  type StageKey = keyof typeof statusContent;
  type RoleKey = keyof (typeof statusContent)[StageKey];
  const bindPaymentBatch = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
      };

      const response = await paymentBatchService.getPaymentBatchData(data);

      // Filter to include only batches with relevant StageText
      const includedStages = ["Initiated", "Pending Approval", "Under Review"];
      const filtered = response?.filter((item: any) =>
        includedStages?.includes(item.status.stageText)
      );

      setRowData(filtered);
    }
  };

  const notifications = rowData?.flatMap((item: any) => {
    debugger
      if (item?.createdBy === currentUserId) return [];
    const stage = item.status.stageText as StageKey;
    const roles: RoleKey[] = ["initiator", "reviewer", "approver"];
    
    return roles
      .map((role) => {
        const roleContent = statusContent[stage]?.[role];
        if (!roleContent || !roleContent.buttonVisible) return null;

        const hasPermission = isAllowed(roleContent.permission);
        if (!hasPermission) return null;

        return {
          id: item.id,
          title: `Batch: ${item.batchName ?? ""}`,
          status: roleContent.title,
          description: roleContent.description,
          link: `/payment-batch/details/${item.id}`,
        };
      })
      .filter(Boolean);
  });

  useEffect(() => {
    bindPaymentBatch();
  }, []);
  useEffect(() => {}, [rowData]);

  return (
    <div className="card">
      <div className="card-header">
        <h3 className="card-title fw-bold fs-4">
          Transactions Pending Authorisation
        </h3>
      </div>
      <div
        className="card-body"
        style={{ maxHeight: "400px", overflowY: "auto" }}
      >
        {notifications?.length === 0 ? (
          <div className="text-muted">No pending payment batches.</div>
        ) : (
          notifications?.map((note: any, index: number) => (
            <Link
              to={note.link}
              key={index}
              className="d-block border p-3 rounded mb-2 bg-light-warning text-dark text-decoration-none"
            >
              <div className="fw-bold">{note.title}</div>
              <div className="small text-muted">{note.status}</div>
              <div className="text-muted small">{note.description}</div>
            </Link>
          ))
        )}
      </div>
    </div>
  );
}
