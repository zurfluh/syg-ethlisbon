const hre = require("hardhat");
module.exports = async({getNamedAccounts, deployments}) => {
    const GalaxyToken = await hre.ethers.getContractFactory("GalaxyToken");
    const galaxyToken = await GalaxyToken.deploy({gasLimit: 80000000});
    console.log("galaxyToken deployed to: ", galaxyToken.address);
    const SpaceMafia = await hre.ethers.getContractFactory("SpaceMafia");
    const spaceMafia = await SpaceMafia.deploy({gasLimit: 80000000});
    await spaceMafia.deployed();
    console.log("spaceMafia deployed to:", spaceMafia.address);
    await galaxyToken.transferOwnership(spaceMafia.address);

    await spaceMafia.initialize(galaxyToken.address, {gasLimit: 8000000});

  };
  module.exports.tags = ['All'];