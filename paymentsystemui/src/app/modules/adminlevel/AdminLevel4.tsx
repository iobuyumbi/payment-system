import { useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import Level4Add from "./cards/Level4Add";
import AdminLevel4List from "./cards/Level4List";
import { Error401 } from "../errors/components/Error401";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";

export function AdminLevel4() {
  const [wardId, setWardId] = useState<any>();
  const [villageData, setVillageData] = useState<any>();
  return (
    <>
      {isAllowed("settings.administrative.view") ? (
        <Content>
          {isAllowed("settings.administrative.add") ? (
            <Level4Add
              wardId={wardId}
              villageData={villageData}
              setWardId={setWardId}
            />
          ) : (
            ""
          )}
          <AdminLevel4List setVillageData={setVillageData} wardId={wardId} />
        </Content>
      ) : (
        <Error401 />
      )}
    </>
  );
}
