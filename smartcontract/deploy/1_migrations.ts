const hre = require("hardhat");
module.exports = async({getNamedAccounts, deployments}) => {

    const SpaceMafia = await hre.ethers.getContractFactory("SpaceMafia");
    const spaceMafia = await SpaceMafia.deploy();
    await spaceMafia.deployed();
    console.log("spaceMafia deployed to:", spaceMafia.address);
    // await spaceMafia.initialize();
    const galaxyToken = await spaceMafia.galaxyToken();
    console.log("galaxyToken is ", galaxyToken)
  };
  module.exports.tags = ['All'];