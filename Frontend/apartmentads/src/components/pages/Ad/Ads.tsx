import React, { useEffect } from "react";
import { API_URL } from "../../../config";
import axios from "axios";
import AdModel from "../../../type/AdModel";
import Ad from "./Ad";
import { Link } from "react-router-dom";

export const Ads = () => {
  const [ads, setAds] = React.useState<AdModel[]>([]);

  useEffect(() => {
    axios.get(`${API_URL}/api/ads`)
      .then((data) => {
        setAds(data.data);
      });
  }, []);

  return <div>
      <div className="h1 text-center">Ads</div>
      <hr />
      <div className="container pb-2">
        <Link to="/" className="btn btn-secondary">Back</Link>
      </div>
      <div className="container pb-5">
        <table className="table table-responsive">
          <thead>
            <tr>
              <th className="col-2">Date</th>
              <th className="col-3">Title</th>
              <th className="col-3">Description</th>
              <th className="col-2">Price</th>
              <th className="col-2">Action</th>
            </tr>
          </thead>
          <tbody>
            {
              ads.map((ad: AdModel) => (
                <Ad key={ad.id} {...ad} />
              ))
            }
          </tbody>
        </table>
      </div>
    </div>

  ;
};